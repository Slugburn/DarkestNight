using System;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGameContext : IGiven
    {
        IHeroContext WithHero(string name);
        IHeroContext WithHero();
        IGameContext NecromancerAt(string location);
        INecromancerContext Necromancer { get; }
        IGameContext NextBlight(params string[] blightNames);
        IGameContext Darkness(int value);
        IGameContext DrawEvents(int count);
        IGameContext NextSearchResult(params Find[] results);
        IGameContext NewDay();
        IGameContext NextArtifact(string artifactName);
    }
}