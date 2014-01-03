using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Taint : BlightImpl
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
