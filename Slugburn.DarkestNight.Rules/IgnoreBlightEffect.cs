using System;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    internal class IgnoreBlightEffect : ISource
    {
        public string Name { get; }
        private readonly Func<Blight, bool> _condition;

        public IgnoreBlightEffect(Func<Blight, bool> condition, string name)
        {
            Name = name;
            _condition = condition;
        }

        public bool Match(Blight blight)
        {
            return _condition(blight);
        }
    }
}