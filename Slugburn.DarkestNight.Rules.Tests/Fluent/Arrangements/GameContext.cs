using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Maps;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class GameContext : GivenContext, IGameContext
    {
        public GameContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IHeroContext WithHero(string name)
        {
            return AddHero(HeroFactory.Create(name));
        }

        public IHeroContext WithHero()
        {
            return AddHero(GenericHeroFactory.Create());
        }

        private IHeroContext AddHero(Hero hero)
        {
            var game = GetGame();
            var player = GetPlayer();
            game.AddHero(hero, player);
            game.ActingHero = hero;
            var heroContext = new HeroContext(game, player, hero);
            heroContext.IsTakingTurn();
            return heroContext;
        }

        public INecromancerContext Necromancer => new NecromancerContext(GetGame(), GetPlayer());

        public IGameContext Darkness(int value)
        {
            GetGame().Darkness = value;
            return this;
        }

        public IGameContext DrawEvents(int count)
        {
            var drawnCards = GetGame().Events.Except(new[] {"Renewal"}).Take(count).ToList();
            drawnCards.ForEach(c => GetGame().Events.Remove(c));
            return this;
        }

        public IGameContext NextSearchResult(params Find[] results)
        {
            var maps = GetGame().Maps;
            foreach (var find in results)
                maps.Insert(0, new Map(new BlightType[7], Enumerable.Repeat(find, 6).ToArray()));
            return this;
        }

        public IGameContext NewDay()
        {
            GetGame().StartNewDay();
            return this;
        }

        public IGameContext NextArtifact(string artifactName)
        {
            var deck = GetGame().ArtifactDeck;
            if (!deck.Contains(artifactName))
                throw new ArgumentOutOfRangeException(nameof(artifactName), artifactName);
            deck.Remove(artifactName);
            deck.Insert(0, artifactName);
            return this;
        }

        public IGameContext NextEvent(params string[] eventNames)
        {
            var deck = GetGame().Events;
            foreach (var eventName in eventNames.Reverse())
            {
                deck.Remove(eventName);
                deck.Insert(0, eventName);
            }
            return this;
        }

        public IGameContext NextBlight(params string[] blightNames)
        {
            foreach (var blightName in blightNames)
            {
                var blight = blightName.ToEnum<BlightType>();
                GetGame().Maps.Insert(0, new Map(Enumerable.Repeat(blight, 7).ToArray(), new Find[6]));
            }
            return this;
        }
    }
}