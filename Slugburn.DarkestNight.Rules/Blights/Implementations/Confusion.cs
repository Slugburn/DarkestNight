using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Confusion : Blight
    {
        public Confusion() :base(BlightType.Confusion)
        {
            Name = "Confusion";
            Might = 4;
            EffectText = "While a hero is in the affected location, his Tactic powers have no effect.";
            DefenseText = "Lose a turn.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseTurn();
        }
    }
}
