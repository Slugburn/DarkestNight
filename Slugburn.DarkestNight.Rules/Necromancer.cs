using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public class Necromancer
    {
        private readonly Game _game;

        public Necromancer(Game game)
        {
            _game = game;
        }

        public Location Location { get; set; }

        public void TakeTurn()
        {
            _game.Darkness = IncreaseDarkness();

            // roll to detect and move
            var movementRoll = Die.Roll();
            var detected = _game.Heroes.Where(h => h.Location != Location.Monastery).Where(h => h.Secrecy < movementRoll).ToArray();

            var necromancerSpace = _game.Board[Location];
            if (!detected.Any())
            {
                // move based on roll
                Location = necromancerSpace.MoveChart[movementRoll];
            }
            else
            {
                // move toward closest detected hero
                throw new NotImplementedException();
            }

            var blightsCreated = 1;
            if (_game.Darkness >= 10 && !necromancerSpace.Blights.Any())
                blightsCreated++;
            if (_game.Darkness >= 20 && movementRoll <= 2)
                blightsCreated++;

            _game.CreateBlights(Location, blightsCreated);
        }

        private int IncreaseDarkness()
        {
            var darkness = _game.Darkness;
            
            // increase darkness by one automatically
            darkness++;

            // increase darkness by number of descrations in play
            darkness += _game.Board.Spaces.Sum(space => space.Blights.Count(blight => blight is Desecration));

            if (darkness <= 30) 
                return darkness;

            // cap darkness at 30 but create a blight in the monastery for every darkness above 30
            _game.CreateBlights(Location.Monastery, darkness - 30);
            return 30;
        }
    }
}
