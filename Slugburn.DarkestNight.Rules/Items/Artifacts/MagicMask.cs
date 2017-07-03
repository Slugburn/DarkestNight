using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class MagicMask : Artifact, IModifier
    {
        public MagicMask() : base("Magic Mask")
        {
            Text = "+1d when eluding.";
        }

        public int GetModifier(Hero hero, ModifierType modifierType)
        {
            return modifierType == ModifierType.EludeDice ? 1 :0;
        }
    }
}
