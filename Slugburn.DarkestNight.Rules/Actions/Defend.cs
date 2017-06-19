using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Enemies;
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
                AvailableTargets = hero.Enemies.GetTargetInfo(),
                AvailableTactics = hero.GetAvailableTactics().GetInfo(hero)
            };
            hero.SetRollHandler(this);
            hero.State = HeroState.SelectingTarget;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Enemies != null && hero.Enemies.Any();
        }

        public void HandleRoll(Hero hero)
        {
            var target = hero.ConflictState.SelectedTargets.Single();
            var tactic = hero.ConflictState.SelectedTactic;
            var targetNumber = tactic.Type == TacticType.Fight ? target.FightTarget : target.EludeTarget;
            var result = hero.Roll.Max();
            if (result < targetNumber)
                hero.TakeWound();
        }
    }
}
