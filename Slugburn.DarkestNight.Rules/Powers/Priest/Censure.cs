namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Censure : TacticPower
    {
        public Censure()
            : base(TacticType.Fight, 2)
        {
            Name = "Censure";
            Text = "Fight with 2d.";
        }

        //            public override void Activate()
        //            {
        //                Hero.SetDice(RollType.Fight, 2);
        //            }
    }
}