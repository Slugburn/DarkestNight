using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public interface ITestRoot : IFakeContext
    {
        IGiven Given { get; }
        IWhen When { get; }
        IGiven Configure(Func<IGiven, IGiven> setConditions);
        ITestRoot Then(IVerifiable verifiable);
        T Set<T>(T state);
        T Get<T>();
    }
}