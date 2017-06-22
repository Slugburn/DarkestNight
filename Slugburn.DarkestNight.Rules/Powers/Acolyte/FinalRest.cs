using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FinalRest : TacticPower
    {
        private const string PowerName = "Final Rest";

        public FinalRest() : base(TacticType.Fight)
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new FinalRestTactic {DiceCount = 2});
            hero.AddTactic(new FinalRestTactic {DiceCount = 3});
        }

        private class FinalRestTactic : PowerTactic
        {
            public FinalRestTactic()
            {
                PowerName = FinalRest.PowerName;
                Type = TacticType.Fight;
            }

            public override string Name => $"{PowerName} ({DiceCount} dice)";

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.AddRollHandler(new FinalRestRollHandler());
            }
        }

        private class FinalRestRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (hero.Roll.AdjustedRoll.Any(x => x == 1))
                    hero.LoseGrace();
                hero.RemoveRollHandler(this);
            }
        }

    }
}