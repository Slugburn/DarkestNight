using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    class Spies : BlightImpl
    {
        public Spies() 
        {
            Name = "Spies";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseSecrecy();
        }
    }
}
