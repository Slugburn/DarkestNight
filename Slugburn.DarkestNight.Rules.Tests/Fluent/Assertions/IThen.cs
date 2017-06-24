namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IThen : ITestRoot
    {
        IPlayerExpectation Player { get; }
        IGameExpectation Game { get; }
    }
}