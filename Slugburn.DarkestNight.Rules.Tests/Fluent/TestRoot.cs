using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class TestRoot : ITestRoot
    {
        protected readonly Game _game;
        protected readonly FakePlayer _player;

        public TestRoot(Game game , FakePlayer player)
        {
            _game = game;
            _player = player;
        }

        public IGiven Given => new Given(_game, _player);
        public IWhen When => new When(_game, _player);
        public IThen Then => new Then(_game, _player);

        public IGiven Configure(Func<IGiven, IGiven> setConditions)
        {
            return setConditions(Given);
        }

    }
}