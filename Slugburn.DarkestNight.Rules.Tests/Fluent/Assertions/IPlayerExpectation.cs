namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IPlayerExpectation : IThen
    {
        IPlayerExpectation SeesTarget(params string[] targetNames);
    }
}