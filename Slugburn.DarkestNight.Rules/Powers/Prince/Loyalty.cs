using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Loyalty : BonusPower
    {
        public Loyalty()
        {
            Name = "Loyalty";
            StartingPower = true;
            Text = "+1d when eluding.";
        }

        protected override void OnLearn()
        {
            AddModifier(ModifierType.EludeDice, 1);
        }
    }
}