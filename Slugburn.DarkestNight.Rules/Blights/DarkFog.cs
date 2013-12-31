namespace Slugburn.DarkestNight.Rules.Blights
{
    public class DarkFog : Blight
    {
        public DarkFog() 
        {
            Name = "Dark Fog";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseTurn();
        }
    }
}
