using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.IO
{
    public class BlightData
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public BlightType Type { get; set; }

        public static BlightData Create(IBlight blight)
        {
            return new BlightData
            {
                Id = blight.Id,
                Location = blight.Location,
                Type = blight.Type
            };
        }

        public void Restore(Game game)
        {
            game.AddBlight(Id, Type, Location);
        }
    }
}
