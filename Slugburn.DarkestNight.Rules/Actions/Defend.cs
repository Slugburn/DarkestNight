using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Defend : IAction, IRollHandler
    {
        public string Name => "Defend";

        public void Act(Hero hero)
        {
            hero.ConflictState = new ConflictState
            {
                MaxTarget = 1,
                MinTarget = 1,
                AvailableTargets = hero.GetBlights(),
                AvailableTactics = hero.GetAvailableTactics().GetInfo(hero)
            };
            hero.SetRollHandler(this);
            hero.State = HeroState.SelectingTarget;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.DefendList != null && hero.DefendList.Any();
        }

        public void HandleRoll(Hero hero)
        {
            var blight = hero.ConflictState.Targets.Single();
            var behavior = (Undead) new BlightFactory().Create(blight);
            var tactic = hero.ConflictState.SelectedTactic;
            var targetNumber = tactic.Type == TacticType.Fight ? behavior.FightTarget : behavior.EludeTarget;
            var result = hero.Roll.Max();
            if (result < targetNumber)
                hero.TakeWound();
        }
    }
}
