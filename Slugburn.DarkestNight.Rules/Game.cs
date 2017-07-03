using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Items.Artifacts;
using Slugburn.DarkestNight.Rules.Maps;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Spaces;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules
{
    public class Game
    {
        private volatile int _id;
        private readonly Dictionary<int, IBlight> _blights = new Dictionary<int, IBlight>();
        private readonly Dictionary<string, IBlightSupression> _blightSupressions = new Dictionary<string, IBlightSupression>();

        public Game()
        {
            Heroes = new List<Hero>();
            Triggers = new TriggerRegistry<GameTrigger, Game>(this);
            Board = Board.CreateFor(this);
            Events = EventFactory.GetEventDeck().Shuffle();
            Maps = new MapFactory().CreateMaps().Shuffle();
            MapsDiscard = new List<IMap>();
            Necromancer = new Necromancer(this);
            Darkness = 0;
            ArtifactDeck = Artifact.CreateDeck().Shuffle();
            BlightFactory = new BlightFactory();
        }

        public BlightFactory BlightFactory { get; }

        public List<string> ArtifactDeck { get; set; }

        public List<IPlayer> Players { get; } = new List<IPlayer>();

        public TriggerRegistry<GameTrigger, Game> Triggers { get; }

        public Board Board { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public List<string> Events { get; set; }

        public IList<IMap> Maps { get; set; }
        private List<IMap> MapsDiscard { get; set; }

        public Necromancer Necromancer { get; set; }
        public int Darkness { get; set; }
        public IEnumerable<Hero> AvailableHeroes { get; set; }
        public Hero ActingHero { get; set; }

        public void StartNewDay()
        {
            ActingHero = null;
            foreach (var hero in Heroes)
                hero.StartNewDay();
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
                CreateBlight(location);
        }

        public void CreateBlight(params Location[] locations)
        {
            var map = DrawMapCard();
            foreach (var location in locations)
            {
                var blightType = map.GetBlight(location);
                CreateBlight(location, blightType);
            }
            DiscardMapCard(map);
        }

        public void CreateBlight(Location location, BlightType blightType)
        {
            var id = NextId();
            var blight = BlightFactory.Create(id, blightType);
            _blights[id] = blight;
            var space = Board[location];
            space.AddBlight(blight);
        }

        public void PopulateInitialBlights()
        {
            var locations = Board.Spaces
                .Select(space => space.Location)
                .Where(loc => loc != Location.Monastery);
            CreateBlight(locations.ToArray());
        }

        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
        }

        public void AddHero(Hero hero, IPlayer player)
        {
            Heroes.Add(hero);
            hero.JoinGame(this, player);
            player.AddHero(PlayerHero.FromHero(hero));
        }

        public void IncreaseDarkness(int count = 1)
        {
            Darkness+=count;

            if (Darkness <= 30)
                return;

            // cap darkness at 30 but create a blight in the monastery for every darkness above 30
            CreateBlights(Location.Monastery, Darkness - 30);
            Darkness = 30;
        }

        public IPower GetPower(string name)
        {
            return Heroes.SelectMany(x => x.Powers).SingleOrDefault(x => x.Name == name);
        }

        public Hero GetHero(string heroName)
        {
            if (heroName == null)
                throw new ArgumentNullException(nameof(heroName));
            var hero = Heroes.SingleOrDefault(x => x.Name == heroName);
            if (hero == null)
                throw new ArgumentOutOfRangeException(nameof(heroName), heroName);
            return hero;
        }

        public bool IsBlightSupressed(IBlight blight, Hero hero)
        {
            return _blightSupressions.Values.Any(x => x.IsSupressed(blight, hero));
        }

        public void AddBlightSupression(IBlightSupression blightSupression)
        {
            _blightSupressions.Add(blightSupression.Name, blightSupression);
        }

        public void RemoveBlightSupression(string name)
        {
            _blightSupressions.Remove(name);
        }

        public void DecreaseDarkness()
        {
            Darkness = Math.Max(0, Darkness - 1);
        }

        public void DestroyBlight(Hero hero, int blightId)
        {
            var blight = _blights[blightId];
            var location = blight.Location;
            var space = Board[location];
            // Shrouds prevent any other type of blight from being destroyed
            if (!(blight is Shroud))
                if (space.GetActiveBlights<Shroud>(hero).Any()) return;

            _blights.Remove(blightId);
            space.RemoveBlight(blight);
            Triggers.Send(GameTrigger.BlightDestroyed, location);
        }

        public static ICollection<Location> GetAllLocations()
        {
            return new[]
            {
                Location.Village,
                Location.Castle,
                Location.Forest,
                Location.Monastery,
                Location.Mountains,
                Location.Ruins,
                Location.Swamp
            };
        }

        public IEnumerable<Find> DrawSearchResult(Location location, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var map = Maps.Draw();
                var result = map.GetSearchResult(location);
                yield return result;
            }
        }

        public int NextId()
        {
            return _id++;
        }

        public IBlight GetBlight(int blightId)
        {
            return _blights[blightId];
        }

        public IEnumerable<IBlight> GetBlights()
        {
            return Board.Spaces.SelectMany(s => s.Blights);
        }

        public IEnumerable<T> GetActiveBlights<T>() where T: IBlight
        {
            return Board.Spaces.SelectMany(x => x.GetActiveBlights<T>());
        }

        public IItem CreateItem(string itemName)
        {
            return ItemFactory.Create(NextId(), itemName);
        }
    }
}