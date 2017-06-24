namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IGameExpectation : IThen
    {
        IGameExpectation Darkness(int darkness);
    }
}