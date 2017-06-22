using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FinalRest : TacticPower
    {
        public FinalRest() : base(TacticType.Fight)
        {
            Name = "Final Rest";
            StartingPower = true;
            Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new FinalRestTactic(Name, 2));
            hero.AddTactic(new FinalRestTactic(Name, 3));
        }

        private class FinalRestTactic : PowerTactic, IRollHandler
        {
            public FinalRestTactic(string powerName, int diceCount)
            {
                PowerName = powerName;
                DiceCount = diceCount;
                Type = TacticType.Fight;
            }

            public override string Name => $"{PowerName} ({DiceCount} dice)";

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.AddRollHandler(this);
            }

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