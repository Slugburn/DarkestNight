namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Shroud : Blight
    {
        public Shroud() 
        {
            Name = "Shroud";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.TakeWound();
        }
    }
}
