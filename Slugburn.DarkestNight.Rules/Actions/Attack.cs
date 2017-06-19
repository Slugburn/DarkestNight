using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

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
                AvailableTargets = hero.GetSpace().Blights.GetTargetInfo(),
                MinTarget = 1,
                MaxTarget = 1
            };
            hero.SetRollHandler(this);
            hero.State = HeroState.SelectingTarget;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.GetBlights().Any();
        }

        public void HandleRoll(Hero hero)
        {
            var result = hero.Roll.Max();
            var target = hero.ConflictState.SelectedTargets.Single();
            var blight = (Blight) Enum.Parse(typeof (Blight), target.Name);
            var assignment = BlightDieAssignment.Create(blight, result);
            hero.State = HeroState.AssigningDice;
            hero.AssignDiceToBlights(new[] {assignment});
            hero.IsActionAvailable = false;
        }
    }
}
