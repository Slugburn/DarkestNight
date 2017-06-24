using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class LocationContext
    {
        private readonly ISpace _space;

        public LocationContext(ISpace space)
        {
            _space = space;
        }

        public LocationContext Blight(params string[] blights)
        {
            foreach (var blight in blights.Select(b => b.ToEnum<Blight>()))
                _space.AddBlight(blight);
            return this;
        }
    }
}