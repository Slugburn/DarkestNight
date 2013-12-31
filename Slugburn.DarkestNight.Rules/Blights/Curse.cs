namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Curse : Blight
    {
        public Curse() 
        {
            Name = "Curse";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseGrace();
        }

    }
}
