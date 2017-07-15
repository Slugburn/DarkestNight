using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class AnimalCompanion : TacticPower
    {
        private const string PowerName = "Animal Companion";

        public AnimalCompanion() : base()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Fight with 2 dice. Exhaust if you fail.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new AnimalCompanionTactic(this));
        }

        private class AnimalCompanionTactic : PowerTactic
        {
            public AnimalCompanionTactic(AnimalCompanion power) : base(power, TacticType.Fight, 2)
            {
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler<AnimalCompanionRoll>();
            }
        }
        internal class AnimalCompanionRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (rollState.Successes == 0)
                    hero.GetPower(PowerName).Exhaust(hero);
            }
        }
    }

}