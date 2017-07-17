using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class TestRoot : ITestRoot
    {
        private readonly Dictionary<Type, object> _state = new Dictionary<Type, object>();

        public Game GetGame() => Get<Game>();

        public FakePlayer GetPlayer() => Get<FakePlayer>();

        public TestRoot(Game game, FakePlayer player)
        {
            Set(game);
            Set(player);
        }

        public IGiven Given => new GivenContext(GetGame(), GetPlayer());
        public IWhen When => new WhenContext(GetGame(), GetPlayer());

        public ITestRoot Then(IVerifiable verifiable)
        {
            while (verifiable is IChildVerifiable)
                verifiable = ((IChildVerifiable) verifiable).Parent;
            verifiable.Verify(this);
            return this;
        }

        public IGiven Configure(Func<IGiven, IGiven> setConditions)
        {
            return setConditions(Given);
        }

        public T Set<T>(T state)
        {
            _state[typeof (T)] = state;
            return state;
        }

        public T Get<T>()
        {
            return (T)_state[typeof (T)];
        }

        public Hero GetHero(string heroName)
        {
            var game = GetGame();
            if (heroName != null)
                return game.GetHero(heroName);
            return game.Heroes.Count == 1 ? game.Heroes.Single() : game.ActingHero;
        }

        protected int GetBlightId(string location, string blightType)
        {
            var space = GetGame().Board[location.ToEnum<Location>()];
            var blight = space.Blights.First(x => x.Type == blightType.ToEnum<BlightType>());
            return blight.Id;
        }

    }
}