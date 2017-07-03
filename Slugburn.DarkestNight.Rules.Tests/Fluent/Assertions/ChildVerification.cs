namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions
{
    public abstract class ChildVerification : IChildVerifiable
    {
        protected ChildVerification(IVerifiable parent)
        {
            Parent = parent;
        }

        public abstract void Verify(ITestRoot root);

        public IVerifiable Parent { get; }
    }
}
