using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    class Desecration : BlightImpl
    {
        public Desecration()
        {
            Name = "Desecration";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            // no effect
        }
    }
}
