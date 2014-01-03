using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Curse : BlightImpl
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
