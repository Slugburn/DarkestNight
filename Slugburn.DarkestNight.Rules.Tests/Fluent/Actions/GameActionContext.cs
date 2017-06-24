using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class GameActionContext
    {
        private readonly Game _game;

        public GameActionContext(Game _game)
        {
            this._game = _game;
        }

        public GameActionContext BlightDestroyed(string location, string blight)
        {
            _game.DestroyBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
            return this;
        }
    }
}