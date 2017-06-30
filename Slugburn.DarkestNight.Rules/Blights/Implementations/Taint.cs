using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Taint : Blight
    {
        public Taint() : base(BlightType.Taint)
        {
            Name = "Taint";
            EffectText = "While a hero is in the affected location, he cannot gain Grace. Whenever he would otherwise gain Grace, there is no effect.";
            Might = 5;
            DefenseText = "Lose 1 Grace.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseGrace();
        }
    }
}
