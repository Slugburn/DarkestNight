using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Looters : Enemy
    {
        public Looters()
        {
            Name = "Looters";
            Fight = 0;
            Elude = 4;
        }

        public override void Win(Hero hero)
        {
            hero.GainSecrecy(1, int.MaxValue);
        }

        public override void Failure(Hero hero)
        {
            hero.ExhaustPowers();
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win", "Gain 1 Secrecy");
            yield return new ConflictResult("Failure", "Exhaust all your powers");
        }
    }
}
