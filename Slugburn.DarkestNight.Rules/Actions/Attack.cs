using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Attack : IAction, IRollClient
    {
        private readonly Hero _hero;

        public Attack(Hero hero)
        {
            _hero = hero;
        }

        public bool Act()
        {
            _hero.ConflictState = new ConflictState
            {
                TacticType = TacticType.Fight,
                AvailableFightTactics = _hero.GetAvailableFightTactics().GetInfo(_hero),
                AvailableTargets = _hero.GetSpace().Blights.Select(x => x.Type).ToList(),
                MinTarget = 1,
                MaxTarget = 1
            };
            _hero.SetRollClient(this);
            _hero.State = HeroState.SelectingTarget;
            return true;
        }

        public void EndCombat(IEnumerable<int> roll)
        {
            var result = roll.Max();
            var target = _hero.ConflictState.Targets.Single();
            var assignment = BlightDieAssignment.Create(target, result);
            _hero.State = HeroState.AssigningDice;
            _hero.AssignDiceToBlights(new[] {assignment});
        }
    }
}
