using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Spaces;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class LocationContext : GameContext, ILocationContext
    {
        private readonly Space _space;

        public LocationContext(Game game, FakePlayer player, Space space) : base(game, player)
        {
            _space = space;
        }

        public ILocationContext Blights(params string[] blights)
        {
            var game = GetGame();
            foreach (var blightType in blights.Select(b => b.ToEnum<BlightType>()))
                game.CreateBlight(_space.Location, blightType);
            return this;
        }

        public ILocationContext HasRelic(bool hasRelic = true)
        {
            _space.HasRelic = hasRelic;
            if (!hasRelic)
                _space.RemoveAction(RetrieveRelic.ActionName);
            return this;
        }
    }
}