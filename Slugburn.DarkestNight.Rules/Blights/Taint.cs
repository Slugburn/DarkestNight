namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Taint : Blight
    {
        public Taint() 
        {
            Name = "Taint";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseGrace();
        }
    }
}
