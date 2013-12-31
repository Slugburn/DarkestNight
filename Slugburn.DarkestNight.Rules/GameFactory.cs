using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    class GameFactory
    {
        private readonly BlightFactory _blightFactory;

        public GameFactory(BlightFactory blightFactory)
        {
            _blightFactory = blightFactory;
        }

        public Game Create()
        {
            var game = new Game
            {
                Board = new Board(),
                Events = new List<IEvent>(),
                Maps = new List<IMap>(),
                Heroes = new List<IHero>(),
                Darkness = 0,
            };
            game.Necromancer = new Necromancer(game);

            PopulateInitialBlights(game);

            return game;
        }

        private void PopulateInitialBlights(Game game)
        {
            var locations = game.Board.Spaces.Select(space => space.Location).Where(loc => loc != Location.Monastery);
            game.CreateBlight(locations);
        }
    }
}
