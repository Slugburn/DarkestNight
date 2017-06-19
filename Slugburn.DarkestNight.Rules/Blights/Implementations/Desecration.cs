using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    class Desecration : BlightDetail
    {
        public Desecration() :base(Blight.Desecration)
        {
            Name = "Desecration";
            EffectText = "The Darkness increases one extra point at the start of each Necromancer turn.";
            Might = 4;
            DefenseText = "No effect.";
        }

        public override void Defend(Hero hero)
        {
            // no effect
        }
    }
}
