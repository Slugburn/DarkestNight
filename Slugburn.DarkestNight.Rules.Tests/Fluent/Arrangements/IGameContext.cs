using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGameContext : IGiven
    {
        IHeroContext WithHero(string name);
        IHeroContext WithHero();
        IGameContext NecromancerAt(string location);
        IGameContext NextBlight(params string[] blightNames);
        IGameContext Darkness(int value);
        IGameContext DrawEvents(int count);
        IGameContext NextSearchResult(Find result);
    }
}