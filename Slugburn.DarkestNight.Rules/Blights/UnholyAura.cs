namespace Slugburn.DarkestNight.Rules.Blights
{
    class UnholyAura : Blight
    {
        public UnholyAura() 
        {
            Name = "Unholy Aura";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseGrace();
        }
    }
}
