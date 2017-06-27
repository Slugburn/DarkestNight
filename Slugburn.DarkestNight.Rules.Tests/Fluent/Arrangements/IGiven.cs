using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGiven : ITestRoot
    {
        IGameContext Game { get; }

        ILocationContext Location(string location);
        IHeroContext Hero(string heroName = null);
    }
}