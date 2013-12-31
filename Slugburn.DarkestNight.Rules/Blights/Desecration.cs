namespace Slugburn.DarkestNight.Rules.Blights
{
    class Desecration : Blight
    {
        public Desecration()
        {
            Name = "Desecration";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            // no effect
        }
    }
}
