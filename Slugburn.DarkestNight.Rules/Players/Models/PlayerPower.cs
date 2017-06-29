using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerPower
    {
        public string Name { get; set; }

        public static IEnumerable<PlayerPower> FromPowers(IEnumerable<IPower> powers)
        {
            return powers.Select(FromPower);
        }

        public static PlayerPower FromPower(IPower power)
        {
            return new PlayerPower {Name = power.Name};
        }
    }
}
