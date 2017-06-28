using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class VileMessenger : Enemy
    {
        public VileMessenger()
        {
            Name = "Vile Messenger";
            Fight = 4;
            Elude = 0;
        }

        public override void Failure(Hero hero)
        {
            hero.Game.IncreaseDarkness();
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Failure", "+1 Darkness");
        }
    }
}
