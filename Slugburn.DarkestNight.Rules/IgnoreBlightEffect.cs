using System;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    internal class IgnoreBlightEffect : ISource
    {
        public string Name { get; }
        private readonly Func<IBlight, bool> _condition;

        public IgnoreBlightEffect(Func<IBlight, bool> condition, string name)
        {
            Name = name;
            _condition = condition;
        }

        public bool Match(IBlight blight)
        {
            return _condition(blight);
        }
    }
}