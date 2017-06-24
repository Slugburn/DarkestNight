using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class LocationContext : GivenContext, ILocationContext
    {
        private readonly ISpace _space;

        public LocationContext(Game game, FakePlayer player, ISpace space) : base(game, player)
        {
            _space = space;
        }

        public LocationContext Blights(params string[] blights)
        {
            foreach (var blight in blights.Select(b => b.ToEnum<Blight>()))
                _space.AddBlight(blight);
            return this;
        }
    }
}