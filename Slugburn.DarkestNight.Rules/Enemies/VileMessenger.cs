using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

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

        public override string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            return !isWin ? "+1 Darkness" : null;
        }
    }
}
