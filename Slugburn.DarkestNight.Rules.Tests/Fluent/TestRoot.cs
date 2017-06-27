using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class TestRoot : ITestRoot
    {
        private readonly Game _game;
        private readonly FakePlayer _player;
        private readonly Dictionary<Type, object> _state = new Dictionary<Type, object>();

        public Game GetGame() => _game;

        public FakePlayer GetPlayer() => _player;

        public TestRoot(Game game, FakePlayer player)
        {
            _game = game;
            _player = player;
            Set(game);
            Set(player);
        }

        public IGiven Given => new GivenContext(GetGame(), GetPlayer());
        public IWhen When => new WhenContext(GetGame(), GetPlayer());

        public ITestRoot Then(IVerifiable verifiable)
        {
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
    }
}