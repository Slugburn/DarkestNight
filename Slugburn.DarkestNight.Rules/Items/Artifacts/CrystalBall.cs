using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class CrystalBall : Artifact, IModifier
    {
        public CrystalBall() : base("Crystal Ball")
        {
            Text = "+1d in searches.";
        }

        public int GetModifier(Hero hero, ModifierType modifierType)
        {
            return modifierType == ModifierType.SearchDice ? 1 : 0;
        }
    }
}
