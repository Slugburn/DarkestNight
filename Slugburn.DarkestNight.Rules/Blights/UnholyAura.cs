using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    class UnholyAura : BlightImpl
    {
        public UnholyAura() 
        {
            Name = "Unholy Aura";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseGrace();
        }
    }
}
