using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventDetail
    {
        private List<string> _text = new List<string>();
        private Dictionary<string, EventOption> _options = new Dictionary<string, EventOption>();
        public static EventDetail Create(Action<EventDetailCreation> create)
        {
            var detail = new EventDetail();
            create(new EventDetailCreation(detail));
            return detail;
        }

        public class EventDetailCreation
        {
            private readonly EventDetail _detail;

            public EventDetailCreation(EventDetail detail)
            {
                _detail = detail;
            }

            public EventDetailCreation Text(params string[] text)
            {
                _detail._text.AddRange(text);
                return this;
            }

            public EventDetailCreation Option(string option, string optionText, Func<Hero, bool> condition  = null)
            {
                _detail._options[option] = new EventOption {Text = optionText, Condition = condition};
                return this;
            }
        }
    }
}