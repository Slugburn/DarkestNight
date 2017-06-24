using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public class PlayerConflictExpectation
    {
        private readonly PlayerConflict _conflict;

        public PlayerConflictExpectation(PlayerConflict conflict)
        {
            _conflict = conflict;
        }

        public PlayerConflictExpectation Target(params string[] names)
        {
            _conflict.Targets.Select(x=>x.Name).ShouldBe(names);
            return this;
        }
    }
}