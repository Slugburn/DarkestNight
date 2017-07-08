using System.Linq;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    internal class DefenseRollHandler : IRollHandler
    {
        public RollState HandleRoll(Hero hero, RollState rollState)
        {
            var conflictState = hero.ConflictState;
            conflictState.Roll = rollState.AdjustedRoll;
            var target = conflictState.SelectedTargets.Single();
            target.ResultDie = rollState.Result;
            hero.DisplayConflictState();
            return rollState;
        }

        public void AcceptRoll(Hero hero, RollState rollState)
        {
            var conflictState = hero.ConflictState;
            conflictState.IsRollAccepted = true;
            var tacticType = conflictState.GetTacticType();
            if (tacticType == TacticType.Elude)
                hero.Triggers.Send(HeroTrigger.Eluding);
            hero.ResolveCurrentConflict();
//            var target = conflictState.SelectedTargets.Single();
//            target.ResultDie = rollState.Result;
//            if (rollState.Successes > 0 && target.TacticType == TacticType.Fight)
//                hero.Triggers.Send(HeroTrigger.FightWon);
//            hero.DisplayConflictState();
        }
    }
}