using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Maps;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules
{
    public class Game : ITriggerRegistrar
    {
        private List<IPlayer> _players = new List<IPlayer>();
        public TriggerRegistry<GameTrigger> Triggers { get; }

        public Board Board { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public ICollection<IEvent> Events { get; set; }

        public IList<IMap> Maps { get; set; }
        private List<IMap> MapsDiscard { get; set; }

        public Necromancer Necromancer { get; set; }
        public int Darkness { get; set; }
        public IEnumerable<Hero> AvailableHeroes { get; set; }

        public Game()
        {
            Heroes = new List<Hero>();
            Triggers = new TriggerRegistry<GameTrigger>(this);
            Board = new Board();
            Events = new List<IEvent>();
            Maps = new MapFactory().CreateMaps().Shuffle();
            MapsDiscard = new List<IMap>();
            Necromancer = new Necromancer(this);
            Darkness = 0;
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

        public void PopulateInitialBlights()
        {
            var locations = Board.Spaces
                .Select(space => space.Location)
                .Where(loc => loc != Location.Monastery);
            CreateBlight(locations);
        }

        public void AddPlayer(IPlayer player)
        {
            _players.Add(player);
        }

        public void AddHero(Hero hero, IPlayer player)
        {
            Heroes.Add(hero);
            hero.JoinGame(this, player);
        }

        public int RollDie()
        {
            return _players.First().RollOne();
        }

        public void IncreaseDarkness()
        {
            // increase darkness by one automatically
            Darkness++;

            // increase darkness by number of descrations in play
            Darkness += Board.Spaces.Sum(space => space.Blights.Count(blight => blight is Desecration));

            if (Darkness <= 30)
                return;

            // cap darkness at 30 but create a blight in the monastery for every darkness above 30
            CreateBlights(Location.Monastery, Darkness - 30);
            Darkness = 30;
        }

        public IPower GetPower(string name)
        {
            return Heroes.SelectMany(x=>x.Powers).SingleOrDefault(x=>x.Name==name);
        }

        public Hero GetHero(string name)
        {
            return Heroes.SingleOrDefault(x => x.Name == name);
        }

        public ITriggerHandler GetTriggerHandler(string handlerName)
        {
            return (ITriggerHandler) GetPower(handlerName);
        }
    }
}
