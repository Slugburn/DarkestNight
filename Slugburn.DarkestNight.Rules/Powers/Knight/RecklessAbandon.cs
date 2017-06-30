using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class RecklessAbandon : TacticPower
    {
        public RecklessAbandon() : base()
        {
            Name = "Reckless Abandon";
            Text = "Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new RecklessAbandonTactic {PowerName = Name} );
        }

        internal class RecklessAbandonTactic : PowerTactic
        {
            public RecklessAbandonTactic()
            {
                Type = TacticType.Fight;
                DiceCount = 4;
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler<RecklessAbandonRollHandler>();
            }
        }

        private class RecklessAbandonRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                rollState.TargetNumber = hero.ConflictState.SelectedTargets.Single().TargetNumber;
                var successes = rollState.Successes;
                if (successes < 2)
                    hero.LoseGrace();
            }
        }

    }
}