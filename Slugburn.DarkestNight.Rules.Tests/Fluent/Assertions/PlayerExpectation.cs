using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerExpectation : ThenContext, IPlayerExpectation
    {
        public PlayerExpectation(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerExpectation SeesTarget(params string[] targetNames)
        {
            Assert.That(GetPlayer().State, Is.EqualTo(PlayerState.Conflict));
            var actual = GetPlayer().Conflict.Targets.Select(x => x.Name);
            Assert.That(actual, Is.EquivalentTo(targetNames));
            return this;
        }

        public void Verify()
        {
        }
    }
}