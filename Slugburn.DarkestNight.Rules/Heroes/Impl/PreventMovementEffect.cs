using System;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    internal class PreventMovementEffect
    {
        private readonly Func<Location, bool> _condition;

        public PreventMovementEffect(Func<Location, bool> condition)
        {
            _condition = condition;
        }

        public bool Matches(Location location)
        {
            return _condition(location);
        }
    }
}