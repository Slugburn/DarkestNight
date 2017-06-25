using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Corruption : BlightDetail
    {
        public Corruption() :base(Blight.Corruption)
        {
            Name = "Corruption";
            EffectText = "While a hero is in the affected location, his Bonus powers have no effect.";
            Might = 5;
            DefenseText = "Exhaust all powers.";
        }

        public override void Failure(Hero hero)
        {
            hero.ExhaustPowers();
        }

    }
}
