using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

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

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddRollModifier(new ConsecratedBladeRollModifer());
        }

        private class ConsecratedBladeRollModifer : IRollModifier
        {
            public string Name => PowerName;

            public int GetModifier(Hero hero, RollType rollType)
            {
                if (rollType != RollType.Fight) return 0;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return 0;
                return 1;
            }

        }
    }
}