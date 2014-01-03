using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Acolyte : Hero
    {
        public Acolyte()
        {
            Name = "Acolyte";
            DefaultGrace = 3;
            DefaultSecrecy = 7;
            Powers = new IPower[]
                     {
                         new FinalRest(),
                         new FalseLife(),
                         new DarkVeil(),
                         new BlindingBlack()
                     };
        }

        #region Powers

        class FinalRest : Tactic
        {
            public FinalRest() : base(TacticType.Fight)
            {
                Name = "Final Rest";
                StartingPower = true;
                Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
            }

            public override void Activate()
            {
                Hero.ChooseDice(RollType.Fight, 2, 3)
                    .AfterFight(() =>
                                {
                                    if (Hero.GetLastRoll(RollType.Fight).Any(x => x == 1))
                                        Hero.LoseGrace();
                                });
            }
        }

        class FalseLife : Bonus
        {
            public FalseLife()
            {
                Name = "False Life";
                StartingPower = true;
                Text = "Exhaust at any time while not at the Monastery to gain 1 Grace (up to default).";
            }

            public override void Activate()
            {
                throw new System.NotImplementedException();
            }
        }

        class DarkVeil : Bonus
        {
            public DarkVeil()
            {
                Name = "Dark Veil";
                StartingPower = true;
                Text =
                    "Exhaust at any time to ignore blights' effects until your next turn. *OR* Exhaust after you fail an attack on a blight to ignore its Defense.";
            }

            public override void Activate()
            {
                throw new System.NotImplementedException();
            }
        }

        class BlindingBlack : Bonus
        {
            public BlindingBlack()
            {
                Name = "Blinding Black";
                StartingPower = true;
                Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
            }

            public override void Activate()
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion
    }

}
