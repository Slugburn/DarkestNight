using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Necromancer : IEnemy, ICallbackHandler<object>
    {
        private readonly Game _game;
        public Location RolledDestination { get; private set; }
        public Location Destination { get; private set; }
        public List<Hero> DetectedHeroes { get; private set; }
        public Hero DetectedHero { get; private set; }
        public int MovementRoll { get; private set; }

        public Necromancer(Game game)
        {
            _game = game;
        }

        public Location Location { get; set; } = Location.Ruins;
        public bool IsTakingTurn { get; set; }

        public string Name => "Necromancer";
        public int Fight => 7;
        public int Elude => 6;

        public void Win(Hero hero)
        {
            if (hero.ConflictState.SelectedTactic.Type == TacticType.Elude) return;
            if (hero.GetBlights().Any())
                throw new NotImplementedException();
            else if (hero.HasHolyRelic())
                _game.Win();
        }

        public void Failure(Hero hero)
        {
            hero.TakeWound();
        }

        public void StartTurn()
        {
            IsTakingTurn = true;

            // increase darkness by one plus number of descrations in play
            var darknessIncrease = 1 + _game.GetActiveBlights<Desecration>().Count();
            _game.IncreaseDarkness(darknessIncrease);

            // roll to detect and move
            MovementRoll = Die.Roll();
            DetectedHeroes = _game.Heroes.Where(h => h.Location != Location.Monastery).Where(h => h.Secrecy < MovementRoll).ToList();

            DetermineDestination();
            _game.UpdatePlayerBoard();
        }

        public void DetermineDestination()
        {
            Destination = GetDestination();

            foreach (var player in _game.Players)
            {
                player.DisplayNecromancer(NecromancerModel.Create(this));
                player.State = PlayerState.Necromancer;
            }

            foreach (var hero in _game.Heroes)
                hero.UpdateAvailableCommands();
        }

        public void CompleteTurn()
        {
            Location = Destination;

            var blightsToCreate = 1;
            if (_game.Darkness >= 10 && !_game.Board[Location].Blights.Any())
                blightsToCreate++;
            if (_game.Darkness >= 20 && MovementRoll <= 2)
                blightsToCreate++;

            _game.CreateBlights(Location, blightsToCreate);
            IsTakingTurn = false;
            _game.UpdatePlayerBoard();
            _game.StartNewDay();
        }

        private Location GetDestination()
        {
            var necromancerSpace = _game.Board[Location];
            RolledDestination = necromancerSpace.MoveChart[MovementRoll];
            if (!DetectedHeroes.Any())
            {
                DetectedHero = null;
                // move based on roll
                return RolledDestination;
            }
            // move toward closest detected hero
            var alreadyHere = DetectedHeroes.Where(h => h.Location == Location).ToList();
            if (alreadyHere.Any())
            {
                DetectedHero = alreadyHere.Shuffle().First();
                return Location;
            }

            var adjacentToNecromancer = _game.Board[Location].AdjacentLocations
                // make sure the Necromancer can never move to Monastery
                .Except(new[] {Location.Monastery})
                .ToList();
            var adjacentHeroes = DetectedHeroes.Where(x => adjacentToNecromancer.Contains(x.Location)).ToList();
            if (adjacentHeroes.Any())
            {
                DetectedHero = adjacentHeroes.Shuffle().First();
                var destination = DetectedHero.Location;
                return destination;
            }
            // select one of the remaining detected heroes at random
            DetectedHero = DetectedHeroes.Shuffle().First();
            // destination is going to be a location that is adjacent to both Necromancer and selected hero
            var adjacentToHero = _game.Board[DetectedHero.Location].AdjacentLocations;
            var adjacentToBoth = adjacentToNecromancer.Intersect(adjacentToHero);
            return adjacentToBoth.Shuffle().First();
        }

        public void HandleCallback(Hero hero, object data)
        {
            CompleteTurn();
        }

        public string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            if (!isWin)
                return "Wound";
            return tacticType == TacticType.Fight ? "Sacrifice blight or (with holy relic) Necromancer slain" : "No effect";
        }
    }
}