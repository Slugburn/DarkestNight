using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public class Game
    {
        public Board Board { get; set; }

        public ICollection<IHero> Heroes { get; set; }

        public ICollection<IEvent> Events { get; set; }

        public ICollection<IMap> Maps { get; set; }

        public Necromancer Necromancer { get; set; }
        public int Darkness { get; set; }

        private IMap DrawMapCard()
        {
            throw new NotImplementedException();
        }

        private void DiscardMapCard(IMap map)
        {
            throw new NotImplementedException();
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
                var blightType = map.GetBlightType(location);
                var blight = new BlightFactory().Create(blightType);
                var space = Board[location];
                space.AddBlight(blight);
            }
            DiscardMapCard(map);
        }
    }
}
