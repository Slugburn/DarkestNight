using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Attack : IAction, IRollHandler
    {
        public string Name => "Attack";

        public void Act(Hero hero)
        {
            hero.ValidateState(HeroState.ChoosingAction);
            hero.ConflictState = new ConflictState
            {
                ConflictType = ConflictType.Attack,
                AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero),
                AvailableTargets = hero.GetSpace().Blights.Select(x => x.Type).ToList(),
                MinTarget = 1,
                MaxTarget = 1
            };
            hero.SetRollHandler(this);
            hero.State = HeroState.SelectingTarget;
        }

        public void HandleRoll(Hero hero)
        {
            var result = hero.ConflictState.Roll.Max();
            var target = hero.ConflictState.Targets.Single();
            var assignment = BlightDieAssignment.Create(target, result);
            hero.State = HeroState.AssigningDice;
            hero.AssignDiceToBlights(new[] {assignment});
            hero.IsActionAvailable = false;
        }
    }
}
