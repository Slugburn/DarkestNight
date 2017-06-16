using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class HeroFactory
    {
        private Dictionary<string, Func<Hero>> _activators;

        public HeroFactory()
        {
            _activators = GetType().Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(Hero).IsAssignableFrom(t))
                .ToDictionary(t => ((Hero) Activator.CreateInstance(t)).Name, t => (Func<Hero>) (() => (Hero) Activator.CreateInstance(t)));
        }

        public Hero Create(string name)
        {
            return _activators[name]();
        }
    }
}
