using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class BloodRing : Artifact, IModifier
    {
        public BloodRing() : base("Blood Ring")
        {
            Text = "+1d in fights.";
        }

        public int GetModifier(Hero hero, ModifierType modifierType)
        {
            return modifierType == ModifierType.FightDice ? 1 : 0;
        }
    }
}
