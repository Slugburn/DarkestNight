using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class PowerData
    {
        public string Name { get; set; }
        public bool Exhausted { get; set; }
        public bool? IsActive { get; set; }
        public string Target { get; set; }

        public static PowerData Create(IPower power)
        {
            var activateable = power as IActivateable;
            var targetable = power as ITargetable;
            var data = new PowerData
            {
                Name = power.Name,
                Exhausted = power.IsExhausted,
                IsActive = activateable?.IsActive,
                Target = targetable?.GetTarget()
            };
            return data;
        }

        public void Restore(Hero hero)
        {
            var power = PowerFactory.Create(Name);
            hero.LearnPower(power);
            if (Exhausted)
                power.Exhaust(hero);
            var targetable = power as ITargetable;
            if (Target != null)
                targetable?.SetTarget(Target);
            var activatable = power as IActivateable;
            if (IsActive ?? false)
                activatable?.Activate(hero);
        }
    }
}