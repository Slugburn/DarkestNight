using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public class Game
    {
        public Board Board { get; set; }

        public ICollection<IHero> Heroes { get; set; }

        public ICollection<IEvent> Events { get; set; }

        public IList<IMap> Maps { get; set; }
        private List<IMap> MapsDiscard { get; set; }

        public Necromancer Necromancer { get; set; }
        public int Darkness { get; set; }
        public IEnumerable<IHero> AvailableHeroes { get; set; }

        public Game()
        {
            MapsDiscard = new List<IMap>();
        }

        private IMap DrawMapCard()
        {
            if (!Maps.Any())
            {
                Maps = MapsDiscard.Shuffle();
                MapsDiscard = new List<IMap>();
            }
            var map = Maps.Draw();
            return map;
        }

        private void DiscardMapCard(IMap map)
        {
            MapsDiscard.Add(map);
        }

        public void CreateBlights(Location location, int count)
        {
            for (var i = 0; i < count; i++)
                CreateBlight(new[] {location});
        }

        public void CreateBlight(IEnumerable<Location> locations)
        {
            var map = DrawMapCard();
            foreach (var location in locations)
            {
                var blightType = map.GetBlight(location);
                var blight = new BlightFactory().Create(blightType);
                var space = Board[location];
                space.AddBlight(blight);
            }
            DiscardMapCard(map);
        }
    }
}
