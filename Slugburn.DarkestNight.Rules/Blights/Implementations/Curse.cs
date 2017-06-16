using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Curse : BlightBase
    {
        public Curse() :base(Blight.Curse)
        {
            Name = "Curse";
            EffectText = "A hero that enters the affected location immediately loses 1 Grace.";
            Might = 5;
            DefenseText = "Lose 1 Grace.";
        }

        public override void Defend(Hero hero)
        {
            hero.LoseGrace();
        }

    }
}
