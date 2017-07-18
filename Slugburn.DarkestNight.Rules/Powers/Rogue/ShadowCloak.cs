using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class ShadowCloak : BonusPower
    {
        public ShadowCloak()
        {
            Name = "Shadow Cloak";
            Text = "+1 die when eluding.";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            AddModifier(ModifierType.EludeDice, 1);
        }
    }
}