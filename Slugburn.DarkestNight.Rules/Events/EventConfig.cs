using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventConfig
    {
        public string Text { get; set; }
        public Func<Hero, bool> Condition { get; set; }
    }
}