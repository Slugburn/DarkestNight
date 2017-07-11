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
            var active = power as IActivateable;
            var data = new PowerData
            {
                Name = power.Name,
                Exhausted = power.Exhausted,
                IsActive = active?.IsActive
            };
            var restorable = power as IRestorable;
            restorable?.Save(data);
            return data;
        }

        public void Restore(Hero hero)
        {
            var power = PowerFactory.Create(Name);
            hero.LearnPower(power);
            if (Exhausted)
                power.Exhaust(hero);
            var restorable = power as IRestorable;
            if (restorable != null)
            {
                restorable.Restore(this);
            }
            else if (IsActive ?? false)
            {
                var activatable = power as IActivateable;
                activatable?.Activate(hero);
            }
        }
    }
}