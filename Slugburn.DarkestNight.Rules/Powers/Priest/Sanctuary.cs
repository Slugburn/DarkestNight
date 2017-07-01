using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Sanctuary : TacticPower
    {
        private const string PowerName = "Sanctuary";

        public Sanctuary()
            : base()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Elude with 4d. Lose 1 Secrecy if you succeed.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new SanctuaryTactic());
        }

        internal class SanctuaryTactic : PowerTactic
        {
            public SanctuaryTactic()
            {
                PowerName = Sanctuary.PowerName;
                Type = TacticType.Elude;
                DiceCount = 4;
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler<SantuaryRoll>();
            }

            internal class SantuaryRoll : IRollHandler
            {
                public RollState HandleRoll(Hero hero, RollState rollState)
                {
                    return rollState;
                }

                public void AcceptRoll(Hero hero, RollState rollState)
                {
                    if (rollState.Successes > 0)
                        hero.LoseSecrecy(1, "Enemy");
                }
            }
        }
    }
}