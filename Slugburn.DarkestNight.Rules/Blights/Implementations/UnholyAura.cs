using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    class UnholyAura : Blight
    {
        public UnholyAura() : base(BlightType.UnholyAura)
        {
            Name = "Unholy Aura";
            EffectText = "While a hero is in the affected location, he rolls one fewer die when fighting (to a minimum of 1).";
            Might = 4;
            DefenseText = "Lose 1 Grace.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseGrace();
        }
    }
}
