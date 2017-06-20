using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventDetail
    {
        private readonly List<string> _text = new List<string>();
        private readonly Dictionary<string, EventConfig> _options = new Dictionary<string, EventConfig>();
        public static EventDetail Create(Action<EventDetailCreation> create)
        {
            var detail = new EventDetail();
            create(new EventDetailCreation(detail));
            return detail;
        }

        public List<EventOption> GetOptions(Hero hero)
        {
            if (_options.Any())
            {
                var activeOptions = _options.Where(x => x.Value.Condition == null || x.Value.Condition(hero));
                return activeOptions.Select(x => new EventOption {Code = x.Key, Text = x.Value.Text}).ToList();
            }
            var defaultOption = new EventOption {Code = "cont", Text = "Continue"};
            return new List<EventOption> {defaultOption};
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
                _detail._options[option] = new EventConfig {Text = optionText, Condition = condition};
                return this;
            }
        }

        public void AddOption(string option, string optionText)
        {
            _options.Add(option, new EventConfig {Text = optionText});
        }

        public List<string> GetText()
        {
            return _text;
        }

        private class EventConfig
        {
            public string Text { get; set; }
            public Func<Hero, bool> Condition { get; set; }
        }
    }
}