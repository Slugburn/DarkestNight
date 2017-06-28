using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Enemy : IEnemy
    {
        public static Enemy Create(string name, int fight, int elude)
        {
            return new Enemy {Name = name, Fight = fight, Elude = elude};
        }

        public int Fight { get; set; }
        public string Name { get; set; }
        public int Elude { get; set; }

        public virtual void Win(Hero hero)
        {
            // nothing
        }

        public virtual void Failure(Hero hero)
        {
            hero.TakeWound();
        }

        public virtual IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Failure", "Take wound");
        }
    }
}