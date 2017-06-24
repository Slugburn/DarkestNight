using System;
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

        public Game GetGame() => _game;

        public FakePlayer GetPlayer() => _player;

        public TestRoot(Game game, FakePlayer player)
        {
            _game = game;
            _player = player;
        }

        public IGiven Given => new GivenContext(GetGame(), GetPlayer());
        public IWhen When => new WhenContext(GetGame(), GetPlayer());
        public IThen Then() => new ThenContext(GetGame(), GetPlayer());

        public ITestRoot Then(IVerifiable verifiable)
        {
            verifiable.Verify(this);
            return this;
        }

        public IGiven Configure(Func<IGiven, IGiven> setConditions)
        {
            return setConditions(Given);
        }
    }
}