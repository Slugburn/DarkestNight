using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerBlight
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string BlightType { get; set; }

        public static PlayerBlight FromBlight(IBlight blight)
        {
            return new PlayerBlight
            {
                Id = blight.Id,
                Location = blight.Location.ToString(),
                BlightType = blight.Type.ToString(),
            };
        }

    }
}
