using System.Linq;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Enemies
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
            _game.IncreaseDarkness();

            // roll to detect and move
            var movementRoll = _game.RollDie();
            var detected = _game.Heroes.Where(h => h.Location != Location.Monastery).Where(h => h.Secrecy < movementRoll).ToList();

            var necromancerSpace = _game.Board[Location];
            var rollLocation = necromancerSpace.MoveChart[movementRoll];
            if (!detected.Any())
            {
                // move based on roll
                Location = rollLocation;
            }
            else
            {
                // move toward closest detected hero
                var handled = _game.Triggers.Handle(GameTrigger.NecromancerDetectsHeroes);
                // TODO: update to find closest
                Location = handled
                    ? detected.First().Location 
                    : rollLocation;
            }

            var blightsCreated = 1;
            if (_game.Darkness >= 10 && !necromancerSpace.Blights.Any())
                blightsCreated++;
            if (_game.Darkness >= 20 && movementRoll <= 2)
                blightsCreated++;

            _game.CreateBlights(Location, blightsCreated);
        }
    }
}
