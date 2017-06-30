using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Tracker : Enemy
    {
        public Tracker()
        {
            Name = "Tracker";
            Fight = 4;
            Elude = 5;
        }

        public override void Win(Hero hero)
        {
            var tacticType = hero.ConflictState.SelectedTactic.Type;
            if (tacticType == TacticType.Fight)
                hero.LoseSecrecy("Enemy");
        }

        public override void Failure(Hero hero)
        {
            hero.LoseSecrecy(2, "Enemy");
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win fight", "Lose 1 Secrecy");
            yield return new ConflictResult("Win elude", "No effect");
            yield return new ConflictResult("Failure", "Lose 2 Secrecy");
        }
    }
}
