using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Sanctuary : TacticPower
    {
        public Sanctuary()
            : base(TacticType.Elude, 4)
        {
            Name = "Sanctuary";
            StartingPower = true;
            Text = "Elude with 4d. Lose 1 Secrecy if you succeed.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.State == HeroState.Eluding;
        }

        //            public override void Activate()
        //            {
        //                Hero.SetDice(RollType.Elude, 4);
        //            }
    }
}