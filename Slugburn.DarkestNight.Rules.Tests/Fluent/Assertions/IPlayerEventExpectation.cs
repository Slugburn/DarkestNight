namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IPlayerEventExpectation : IPlayerExpectation
    {
        PlayerEventExpectation HasBody(string title, int fate, string text);
        PlayerEventExpectation HasOptions(params string[] options);
        PlayerEventExpectation ActiveRow(string text, string subText = null);
    }
}