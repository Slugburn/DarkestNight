using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class PowerData
    {
        public static PowerData Create(IPower power)
        {
            var active = power as IActivateable;
            return new PowerData
            {
                Name = power.Name,
                Exhausted = power.Exhausted,
                IsActive = active?.IsActive
            };
        }

        public string Name { get; set; }
        public bool Exhausted { get; set; }
        public bool? IsActive { get; set; }
    }
}
