using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class DeathMask : BonusPower
    {
        private const string PowerName = "Death Mask";

        public DeathMask()
        {
            Name = PowerName;
            Text =
                "You may choose not to lose Secrecy for attacking a blight (including use of the Call to Death power) or for starting your turn at the Necromancer's location.";
        }

        protected override void OnLearn()
        {
            Owner.Triggers.Add(HeroTrigger.LosingSecrecy, Name, new DeathMaskTriggerHandler());
        }

        private class DeathMaskTriggerHandler : ITriggerHandler<Hero>
        {
            public Task HandleTriggerAsync(Hero registrar, string source, TriggerContext context)
            {
                var hero = registrar;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return Task.CompletedTask;
                var sourceName = context.GetState<string>();
                if (sourceName == "Attack" || sourceName == "Necromancer")
                    context.Cancel = true;
                return Task.CompletedTask;
            }

        }
    }
}