using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class PowerModel
    {
        public string Name { get; set; }

        public static IEnumerable<PowerModel> FromPowers(IEnumerable<IPower> powers)
        {
            return powers.Select(FromPower);
        }

        public static PowerModel FromPower(IPower power)
        {
            return new PowerModel {Name = power.Name};
        }
    }
}
