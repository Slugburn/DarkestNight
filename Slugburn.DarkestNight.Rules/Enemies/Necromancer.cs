using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Necromancer : IEnemy
    {
        private readonly Game _game;
        public List<Hero> DetectedHeroes { get; private set; }
        public int MovementRoll { get; private set; }

        public Necromancer(Game game)
        {
            _game = game;
        }

        public Location Location { get; set; }
        public bool IsActing { get; set; }

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
            IsActing = true;
            _game.IncreaseDarkness();

            // roll to detect and move
            MovementRoll = Die.Roll();
            DetectedHeroes = _game.Heroes.Where(h => h.Location != Location.Monastery).Where(h => h.Secrecy < MovementRoll).ToList();

            DetermineDestination();
        }

        public void DetermineDestination()
        {
            var destination = GetDestination();

            foreach (var player in _game.Players)
            {
                player.DisplayNecromancer(new PlayerNecromancer(MovementRoll, destination, DetectedHeroes.Select(x => x.Name).ToList()));
                player.State = PlayerState.Necromancer;
            }

            foreach (var hero in _game.Heroes)
                hero.UpdateAvailableActions();
        }

        public void CompleteTurn()
        {
            var destination = GetDestination();
            Location = destination;

            var blightsCreated = 1;
            if (_game.Darkness >= 10 && !_game.Board[Location].Blights.Any())
                blightsCreated++;
            if (_game.Darkness >= 20 && MovementRoll <= 2)
                blightsCreated++;

            _game.CreateBlights(Location, blightsCreated);
            IsActing = false;
        }

        private Location GetDestination()
        {
            var necromancerSpace = _game.Board[Location];
            var rollLocation = necromancerSpace.MoveChart[MovementRoll];
            Location destination;
            if (!DetectedHeroes.Any())
            {
                // move based on roll
                destination = rollLocation;
            }
            else
            {
                // move toward closest detected hero
                var handled = _game.Triggers.Send(GameTrigger.NecromancerDetectsHeroes);
                // TODO: update to find closest
                destination = handled
                    ? DetectedHeroes.First().Location
                    : rollLocation;
            }
            return destination;
        }
    }
}