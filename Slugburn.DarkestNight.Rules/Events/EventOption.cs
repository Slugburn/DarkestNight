using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    internal class EventOption
    {
        public EventOption(string code, string text, Func<Hero, bool> condition)
        {
            Code = code;
            Text = text;
            Condition = condition;
        }

        public string Code { get; set; }
        public string Text { get; }
        public Func<Hero, bool> Condition { get; }
    }
}