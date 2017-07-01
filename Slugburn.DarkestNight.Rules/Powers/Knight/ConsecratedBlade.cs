using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class ConsecratedBlade : BonusPower
    {
        private const string PowerName = "Consecrated Blade";

        public ConsecratedBlade()
        {
            Name = PowerName;
            Text = "+1 dice in fights.";
        }

        protected override void OnLearn()
        {
            AddModifier(ModifierType.FightDice, 1);
        }
    }
}