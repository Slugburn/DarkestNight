using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class PowerModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public bool IsExhausted { get; set; }
        public bool IsActive { get; set; }

        public static List<PowerModel> Create(IEnumerable<IPower> powers)
        {
            return powers.Select(Create).ToList();
        }

        public static PowerModel Create(IPower power)
        {
            return new PowerModel
            {
                Name = power.Name,
                Text = power.Text,
                Html = power.Html,
                IsExhausted = power.Exhausted,
                IsActive = (power as IActivateable)?.IsActive ?? false
            };
        }

    }
}
