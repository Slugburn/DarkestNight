using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Curse : Blight
    {
        public Curse() :base(BlightType.Curse)
        {
            Name = "Curse";
            EffectText = "A hero that enters the affected location immediately loses 1 Grace.";
            Might = 5;
            DefenseText = "Lose 1 Grace.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseGrace();
        }

    }
}
