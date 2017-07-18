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
            var target = conflictState.SelectedTargets.Single();
            target.ResultDie = rollState.Result;
            hero.DisplayConflictState();
            return rollState;
        }

        public async void AcceptRoll(Hero hero, RollState rollState)
        {
            var conflictState = hero.ConflictState;
            conflictState.IsRollAccepted = true;
            var tacticType = conflictState.GetTacticType();
            if (tacticType == TacticType.Elude)
                await hero.Triggers.Send(HeroTrigger.Eluding);
            hero.ResolveCurrentConflict();
        }
    }
}