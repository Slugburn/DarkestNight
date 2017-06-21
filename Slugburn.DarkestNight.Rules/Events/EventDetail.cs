using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventDetail
    {
        private readonly List<EventEnemy> _enemies = new List<EventEnemy>();
        private readonly Dictionary<string, EventConfig> _options = new Dictionary<string, EventConfig>();
        private readonly List<EventRow> _rows = new List<EventRow>();

        private string _text;

        private EventDetail()
        {
        }

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

        public string GetText()
        {
            return _text;
        }

        public List<EventEnemy> GetEnemies()
        {
            return _enemies.ToList();
        }

        public List<EventRow> GetRows()
        {
            return _rows.ToList();
        }

        public class EventDetailCreation
        {
            private readonly EventDetail _detail;

            public EventDetailCreation(EventDetail detail)
            {
                _detail = detail;
            }

            public EventDetailCreation Text(string text)
            {
                _detail._text = text;
                return this;
            }

            public EventDetailCreation Option(string option, string optionText, Func<Hero, bool> condition = null)
            {
                _detail._options[option] = new EventConfig {Text = optionText, Condition = condition};
                return this;
            }

            public EventDetailCreation Row(int min, int max, string text, string subText = null)
            {
                _detail._rows.Add(new EventRow {Min = min, Max = max, Text = text, SubText = subText});
                return this;
            }

            public EventDetailCreation Row(int number, string text, string subText = null)
            {
                _detail._rows.Add(new EventRow {Min = number, Max = number, Text = text, SubText = subText});
                return this;
            }

            public EventDetailCreation Enemy(int min, int max, string name)
            {
                _detail._enemies.Add(new EventEnemy {Min = min, Max = max, Name = name});
                return this;
            }

            public EventDetailCreation Enemy(int number, string name)
            {
                return Enemy(number, number, name);
            }

            public EventDetailCreation Enemy(string name)
            {
                return Enemy(0, 0, name);
            }
        }

        private class EventConfig
        {
            public string Text { get; set; }
            public Func<Hero, bool> Condition { get; set; }
        }
    }
}