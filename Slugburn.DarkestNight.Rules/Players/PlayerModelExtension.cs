using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Players
{
    public static class PlayerModelExtension
    {
        public static List<PlayerPower> ToPlayerPowers(this IEnumerable<IPower> powers)
        {
            return powers.Select(PlayerPower.FromPower).ToList();
        }
    }
}
