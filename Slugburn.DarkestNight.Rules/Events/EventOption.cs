using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    internal class EventOption
    {
        public string Text { get; set; }
        public Func<Hero, bool> Condition { get; set; }
    }
}