using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Cultist : Enemy
    {
        public Cultist()
        {
            Name = "Cultist";
            Fight = 5;
            Elude = 3;
        }

        public override void Win(Hero hero)
        {
            if (hero.ConflictState.SelectedTactic.Type == TacticType.Fight)
                hero.Game.DecreaseDarkness();
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win fight", "-1 Darkness");
            yield return new ConflictResult("Win elude", "No effect");
            yield return new ConflictResult("Failure", "Take wound");
        }
    }
}