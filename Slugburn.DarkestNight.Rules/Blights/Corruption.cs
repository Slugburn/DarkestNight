using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Corruption : BlightImpl
    {
        public Corruption() 
        {
            Name = "Corruption";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.ExhaustPowers();
        }

    }
}
