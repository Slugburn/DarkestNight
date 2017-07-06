using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Attack : StandardAction
    {

        public Attack() : base("Attack")
        {
            Text = "Choose a blight in your location and fight it.\n"
                   + "If you win, the blight is destroyed; otherwise, you suffer the blight’s defense.\n"
                   + "Succeed or fail, you lose 1 Secrecy for revealing yourself.";
        }

        public override bool IsAvailable(Hero hero)
        {
            return base.IsAvailable(hero) && hero.GetBlights().Any();
        }

        public override void Execute(Hero hero)
        {
            hero.SetRoll(RollBuilder.Create<AttackRoll>());
            var conflictState = new ConflictState
            {
                ConflictType = ConflictType.Attack,
                MinTarget = 1,
                MaxTarget = 1,
                AvailableTargets = hero.Space.Blights.GetTargetInfo(hero.Game)
            };
            hero.ConflictState = conflictState;
            // hero.ConflictState.ConflictType needs to be set before calling hero.GetAvailableFightTactics()
            conflictState.AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero);
            hero.DisplayConflictState();
        }


        private class AttackRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                rollState.TargetNumber = hero.ConflictState.SelectedTargets.Single().TargetNumber;
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.ConflictState.Resolve(hero);
            }
        }
    }
}
