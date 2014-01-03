using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    internal class TriggeredAction : IPowerEffect
    {
        public TriggeredAction(Trigger afterRoll, string description, System.Action<IHero> action)
        {
            throw new NotImplementedException();
        }

        public Func<IHero,bool> Condition { get; set; }
        
        public bool Active { get; set; }
    }
}