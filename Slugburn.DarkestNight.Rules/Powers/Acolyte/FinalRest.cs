using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FinalRest : TacticPower
    {
        private const string PowerName = "Final Rest";

        public FinalRest() : base()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new FinalRestTactic {DiceCount = 2});
            Owner.AddTactic(new FinalRestTactic {DiceCount = 3});
        }

        private class FinalRestTactic : PowerTactic
        {
            public FinalRestTactic()
            {
                PowerName = FinalRest.PowerName;
                Type = TacticType.Fight;
            }

            public override string Name => $"{PowerName} [{DiceCount}d]";

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler<FinalRestRoll>();
            }
        }

        private class FinalRestRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (hero.CurrentRoll.AdjustedRoll.Any(x => x == 1))
                    hero.LoseGrace();
            }
        }

    }
}