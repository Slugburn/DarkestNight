using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class DarkFog : BlightImpl
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
