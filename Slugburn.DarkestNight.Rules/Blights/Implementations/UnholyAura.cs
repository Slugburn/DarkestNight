using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    class UnholyAura : BlightDetail
    {
        public UnholyAura() : base(Blight.UnholyAura)
        {
            Name = "Unholy Aura";
            EffectText = "While a hero is in the affected location, he rolls one few die when fighting (to a minimum of 1).";
            Might = 4;
            DefenseText = "Lose 1 Grace.";
        }

        public override void Defend(Hero hero)
        {
            hero.LoseGrace();
        }
    }
}
