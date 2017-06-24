namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IGameExpectation : IThen
    {
        IGameExpectation NecromancerLocation(string location);
        IGameExpectation Darkness(int darkness);
        IGameExpectation EventDeckIsReshuffled();
    }
}