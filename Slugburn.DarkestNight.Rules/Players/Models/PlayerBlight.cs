using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerBlight
    {
        public PlayerBlight(IBlight blight)
        {
            Id = blight.Id;
            Location = blight.Location.ToString();
            BlightType = blight.Type.ToString();
            Might = blight.Might;
        }

        public PlayerBlight()
        {
        }

        public int Id { get; set; }
        public string Location { get; set; }
        public string BlightType { get; set; }
        public int Might { get; set; }

        public static List<PlayerBlight> Create(IEnumerable<IBlight> blights)
        {
            return blights.Select(blight=>new PlayerBlight(blight)).ToList();
        }
    }
}
