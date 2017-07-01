using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Tranquility : BonusPower
    {
        public Tranquility()
        {
            Name = "Tranquility";
            Text = "+3 to default Grace.";
        }

        protected override void OnLearn()
        {
            AddModifier(ModifierType.DefaultGrace, 3);
        }
    }
}