using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public static class RollBuilder
    {
        public static IRollStateCreation Create(IRollHandler rollHandler)
        {
            return new RollStateCreation(rollHandler);
        }

        public static IRollStateCreation Create<THandler>() where THandler : IRollHandler, new()
        {
            return Create(new THandler());
        }

        internal class RollStateCreation : IRollStateCreation
        {
            private readonly IRollHandler _handler;
            private ModifierType _type;
            private string _baseName;
            private int _baseDiceCount;
            private int _targetNumber;

            public RollStateCreation(IRollHandler handler)
            {
                _handler = handler;
            }

            public IRollStateCreation Type(ModifierType type)
            {
                _type = type;
                return this;
            }

            public IRollStateCreation Base(string name, int diceCount)
            {
                _baseName = name;
                _baseDiceCount = diceCount;
                return this;
            }

            public IRollStateCreation Target(int targetNumber)
            {
                _targetNumber = targetNumber;
                return this;
            }

            public RollState Create(Hero hero)
            {
                var state = new RollState(hero)
                {
                    ModifierType = _type,
                    BaseName = _baseName,
                    BaseDiceCount = _baseDiceCount,
                    TargetNumber = _targetNumber
                };
                state.SetPrimaryRollHandler(_handler);
                return state;
            }
        }
    }
}
