using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class AttackNecromancer : StandardAction
    {
        public const string ActionName = "Attack Necromancer";

        public AttackNecromancer() : base(ActionName)
        {
            Text = "Failing any combat with the Necromancer results in a wound.\n"
                   + "If there are any blights in the location, successfully fighting the Necromancer will\n"
                   + "entice him to sacrifice one of them so he can escape; destroy one of your choice.\n"
                   + "Successfully fighting the Necromancer when there are no blights in the location\n"
                   + "slays him and wins the game—but only if you are holding a holy relic when you do so.";
        }

        public override void Execute(Hero hero)
        {
            var necromancer = hero.Game.Necromancer;
            var availableTargets = new [] {necromancer}.GetTargetInfo();
            var selectedTargets = availableTargets.Select(x=> new ConflictTarget(necromancer, x, TacticType.Fight)).ToList();
            var conflictState = new ConflictState
            {
                ConflictType = ConflictType.Attack,
                SelectedTargets =  selectedTargets,
                MinTarget = 1,
                MaxTarget = 1,
                AvailableTargets = availableTargets
            };
            hero.ConflictState = conflictState;
            hero.SetRoll(RollBuilder.Create<FightNecromancerRoll>());
            // hero.ConflictState.ConflictType needs to be set before calling hero.GetAvailableFightTactics()
            conflictState.AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero);
            hero.DisplayConflictState();
        }

        public override bool IsAvailable(Hero hero)
        {
            return base.IsAvailable(hero) && hero.Location == hero.Game.Necromancer.Location;
        }

        private class FightNecromancerRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                rollState.TargetNumber = hero.Game.Necromancer.Fight;
                var target = hero.ConflictState.SelectedTargets.Single();
                target.ResultDie = rollState.Result;
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.ResolveCurrentConflict();
            }
        }

    }
}