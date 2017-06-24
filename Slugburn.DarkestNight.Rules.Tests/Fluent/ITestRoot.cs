using System;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public interface ITestRoot : IFakeRollContext
    {
        IGiven Given { get; }
        IWhen When { get; }
        IThen Then();
        IGiven Configure(Func<IGiven, IGiven> setConditions);
        ITestRoot Then(IVerifiable verifiable);
    }
}