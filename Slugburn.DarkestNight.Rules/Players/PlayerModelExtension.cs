using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Players
{
    public static class PlayerModelExtension
    {
        public static List<PowerModel> ToPlayerPowers(this IEnumerable<IPower> powers)
        {
            return powers.Select(PowerModel.Create).ToList();
        }
    }
}
