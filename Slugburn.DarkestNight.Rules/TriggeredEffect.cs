using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    internal class TriggeredEffect : IPowerEffect
    {
        public TriggeredEffect(Trigger trigger, string description, System.Action<IHero> effect, ISource source)
        {
            throw new NotImplementedException();
        }

        public bool Active { get; set; }
    }
}