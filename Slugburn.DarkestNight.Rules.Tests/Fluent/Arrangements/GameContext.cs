using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
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

        public IGameContext WithHero(string name, Action<HeroContext> def = null)
        {
            AddHero(HeroFactory.Create(name), def);
            return this;
        }

        public IGameContext WithHero(Action<HeroContext> def = null)
        {
            AddHero(GenericHeroFactory.Create(), def);
            return this;
        }

        public IGameContext NecromancerIn(string location)
        {
            GetGame().Necromancer.Location = location.ToEnum<Location>();
            return this;
        }

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

        public IGameContext NextSearchResult(Find result)
        {
            GetGame().Maps.Insert(0, new Map(new Blight[7], Enumerable.Repeat(result, 6).ToArray()));
            return this;
        }

        public IGameContext NextBlight(params string[] blightNames)
        {
            foreach (var blightName in blightNames)
            {
                var blight = blightName.ToEnum<Blight>();
                GetGame().Maps.Insert(0, new Map(Enumerable.Repeat(blight, 7).ToArray(), new Find[6]));
            }
            return this;
        }

        private void AddHero(Hero hero, Action<HeroContext> def)
        {
            GetGame().AddHero(hero, GetPlayer());
            var ctx = new HeroContext(hero);
            def?.Invoke(ctx);
            GetGame().ActingHero = hero;
            GetPlayer().ActiveHero = hero.Name;
        }
    }
}