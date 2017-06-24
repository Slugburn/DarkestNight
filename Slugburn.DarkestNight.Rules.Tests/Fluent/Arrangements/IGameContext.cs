using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGameContext : IGiven
    {
        IGameContext WithHero(string name, Action<HeroContext> def = null);
        IGameContext WithHero(Action<HeroContext> def = null);
        IGameContext NecromancerLocation(string location);
        IGameContext NextBlight(string blightName);
        IGameContext Darkness(int value);
        IGameContext DrawEvents(int count);
        IGameContext NextSearchResult(Find result);
    }
}