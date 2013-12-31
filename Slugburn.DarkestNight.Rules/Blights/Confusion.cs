namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Confusion : Blight
    {
        public Confusion() 
        {
            Name = "Confusion";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseTurn();
        }
    }
}
