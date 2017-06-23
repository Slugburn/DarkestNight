using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerPower
    {
        public string Name { get; set; }

        public static PlayerPower FromPower(IPower power)
        {
            return new PlayerPower {Name = power.Name};
        }
    }
}
