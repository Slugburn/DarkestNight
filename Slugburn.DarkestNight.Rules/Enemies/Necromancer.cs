using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Necromancer : IEnemy
    {
        private readonly Game _game;
        public Location Destination { get; private set; }
        public List<Hero> DetectedHeroes { get; private set; }
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
            if (hero.GetBlights().Any())
                throw new NotImplementedException();
            if (hero.HasHolyRelic())
                throw new NotImplementedException();
        }

        public void Failure(Hero hero)
        {
            hero.TakeWound();
        }

        public IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win fight", "Sacrifice blight or (with holy relic) Necromancer slain");
            yield return new ConflictResult("Win elude", "No effect");
            yield return new ConflictResult("Failure", "Take wound");
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
                player.DisplayNecromancer(NecromancerModel.From(this));
                player.State = PlayerState.Necromancer;
            }

            foreach (var hero in _game.Heroes)
                hero.UpdateAvailableCommands();
        }

        public void CompleteTurn()
        {
            Destination = GetDestination();
            Location = Destination;

            var blightsCreated = 1;
            if (_game.Darkness >= 10 && !_game.Board[Location].Blights.Any())
                blightsCreated++;
            if (_game.Darkness >= 20 && MovementRoll <= 2)
                blightsCreated++;

            _game.CreateBlights(Location, blightsCreated);
            IsTakingTurn = false;
            _game.UpdatePlayerBoard();
            _game.StartNewDay();
        }

        private Location GetDestination()
        {
            var necromancerSpace = _game.Board[Location];
            var rollLocation = necromancerSpace.MoveChart[MovementRoll];
            if (!DetectedHeroes.Any())
            {
                // move based on roll
                return rollLocation;
            }
            // move toward closest detected hero
            var alreadyHere = DetectedHeroes.Any(h => h.Location == Location);
            if (alreadyHere)
                return Location;

            var adjacentToNecromancer = _game.Board[Location].AdjacentLocations
                // make sure the Necromancer can never move to Monastery
                .Except(new[] {Location.Monastery})
                .ToList();
            var adjacentHeroes = DetectedHeroes.Where(x => adjacentToNecromancer.Contains(x.Location)).ToList();
            if (adjacentHeroes.Any())
            {
                var destination = adjacentHeroes.Shuffle().First().Location;
                return destination;
            }
            // select one of the remaining detected heroes at random
            var selected = DetectedHeroes.Shuffle().First();
            // destination is going to be a location that is adjacent to both Necromancer and selected hero
            var adjacentToHero = _game.Board[selected.Location].AdjacentLocations;
            var adjacentToBoth = adjacentToNecromancer.Intersect(adjacentToHero);
            return adjacentToBoth.Shuffle().First();
        }
    }
}