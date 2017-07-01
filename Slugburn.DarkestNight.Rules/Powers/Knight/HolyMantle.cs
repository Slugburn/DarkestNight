using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class HolyMantle : BonusPower
    {
        private const string PowerName = "Holy Mantle";

        public HolyMantle()
        {
            Name = PowerName;
            Text = "+1 to default Grace. Add 1 to each die when praying.";
        }

        protected override void OnLearn()
        {
            AddModifier(ModifierType.DefaultGrace, 1);
            Owner.Triggers.Add(HeroTrigger.Rolled, Name, new HolyMantleAfterRoll());
        }

        private class HolyMantleAfterRoll : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                if (context.GetState<ModifierType>() != ModifierType.PrayDice) return;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return;

                // Increase each die by 1
                hero.CurrentRoll.AdjustedRoll = hero.CurrentRoll.AdjustedRoll.Select(x => x + 1).ToList();
            }
        }
    }
}