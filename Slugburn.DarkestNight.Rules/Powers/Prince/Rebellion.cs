namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Rebellion : TacticPower
    {
        public Rebellion()
            : base()
        {
            Name = "Rebellion";
            Text = "Fight with 3d when attacking a blight or the Necromancer";
        }

        //            public override void Activate()
        //            {
        //                Hero.SetDice(RollType.Fight, 3);
        //            }
    }
}