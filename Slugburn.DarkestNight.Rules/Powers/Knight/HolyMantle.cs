using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class HolyMantle : Bonus
    {
        private const string PowerName = "Holy Mantle";

        public HolyMantle()
        {
            Name = PowerName;
            Text = "+1 to default Grace. Add 1 to each die when praying.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.DefaultGrace += 1;
            hero.Triggers.Register(HeroTrigger.AfterRoll, new HolyMantleAfterRoll());
        }

        private class HolyMantleAfterRoll : ITriggerHandler<Hero>
        {
            public string Name => PowerName;
            public void HandleTrigger(Hero registrar, TriggerContext context)
            {
                var hero = registrar;
                if (hero.State != HeroState.Praying) return;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return;

                // Increase each die by 1
                hero.Roll = hero.Roll.Select(x => x + 1).ToList();
            }
        }
    }
}