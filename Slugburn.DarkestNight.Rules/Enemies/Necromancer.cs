using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Necromancer : IEnemy
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
            var movementRoll = Die.Roll();
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
                var handled = _game.Triggers.Send(GameTrigger.NecromancerDetectsHeroes);
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

        public string Name => "Necromancer";
        public int Fight => 7;
        public int Elude => 6;
        public void Win(Hero hero)
        {
            if (hero.GetBlights().Any())
                throw new System.NotImplementedException();
            else if (hero.HasHolyRelic())
                throw new System.NotImplementedException();
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

        public void ResolveAttack(int result, Hero hero)
        {
            if (result >= Fight)
                Win(hero);
            else
                Failure(hero);
        }
    }
}
