namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public interface IVerifiable
    {
        void Verify(ITestRoot root);
    }

    public interface IChildVerifiable : IVerifiable
    {
        IVerifiable Parent { get; }
    }
}
