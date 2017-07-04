using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventDetail
    {
        private readonly List<EventRow> _rows = new List<EventRow>();
        private readonly List<EventEnemy> _enemies = new List<EventEnemy>();
        private readonly Dictionary<string, EventOption> _options = new Dictionary<string, EventOption>();

        private string _text;
        private Func<Hero, int,int, bool> _rowSelector;
        private Func<Hero, int> _indexSelector;

        private EventDetail(string name, int fate)
        {
            Name = name;
            Fate = fate;
        }

        public string Name { get; }
        public int Fate { get; }

        public static EventDetail Create(string name, int fate, Action<EventDetailCreation> create)
        {
            var detail = new EventDetail(name, fate);
            create(new EventDetailCreation(detail));
            return detail;
        }

        public List<HeroEventOption> GetHeroEventOptions(Hero hero, int? rowIndex)
        {
            // If we haven't been supplied an index, try to use the row index selector
            if (rowIndex == null && _indexSelector != null)
                rowIndex = _indexSelector(hero);
            if (rowIndex != null)
            {
                // Find the matching row. If it has any options, use those
                var row = _rows.SingleOrDefault(r => r.Min <= rowIndex.Value && rowIndex.Value <= r.Max);
                if (row != null && row.Options.Any())
                    return CreateHeroEventOptions(hero, row.Options);
            }
            if (_options.Any())
                return CreateHeroEventOptions(hero, _options.Select(x => x.Value));

            // Create a default option if none are presented
            var defaultOption = new HeroEventOption {Code = "cont", Text = "Continue"};
            return new List<HeroEventOption> {defaultOption};
        }

        private List<HeroEventOption> CreateHeroEventOptions(Hero hero, IEnumerable<EventOption> eventOptions)
        {
            var options = eventOptions.ToList();
            var activeOptions = options.Where(x => x.Condition == null || x.Condition(hero));
            return activeOptions.Select(x => new HeroEventOption {Code = x.Code, Text = x.Text}).ToList();
        }

        public string GetText()
        {
            return _text;
        }

        public List<EventEnemy> GetEnemies()
        {
            return _enemies.ToList();
        }

        public List<HeroEventRow> CreateHeroRows(Hero hero)
        {
            return _rows.Select(row => new HeroEventRow
            {
                Min = row.Min,
                Max = row.Max,
                Text = row.Text,
                SubText = row.SubText,
                IsActive = _rowSelector?.Invoke(hero, row.Min, row.Max) ?? false
            }).Concat(_enemies.Select(e => new HeroEventRow
            {
                Min = e.Min,
                Max = e.Max,
                Text = e.Name,
            })).ToList();
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

            public EventDetailCreation Option(string code, string text, Func<Hero, bool> condition = null)
            {
                _detail._options[code] = new EventOption(code, text, condition);
                return this;
            }


            public EventDetailCreation Row(int min, int max, string text, string subText, Action<EventRowCreation> create = null)
            {
                var row = new EventRow {Min = min, Max = max, Text = text, SubText = subText};
                var ctx = new EventRowCreation(row);
                create?.Invoke(ctx);
                _detail._rows.Add(row);
                return this;
            }

            public EventDetailCreation Row(int number, string text, string subText, Action<EventRowCreation> create = null)
            {
                return Row(number, number, text, subText, create);
            }

            public EventDetailCreation Row(int min, int max, string text, Action<EventRowCreation> create = null)
            {
                return Row(min, max, text, null, create);
            }

            public EventDetailCreation Row(int number, string text, Action<EventRowCreation> create = null)
            {
                return Row(number, number, text, null, create);
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

            public class EventRowCreation
            {
                private readonly EventRow _row;

                internal EventRowCreation(EventRow row)
                {
                    _row = row;
                }

                public EventRowCreation Option(string code, string text, Func<Hero, bool> condition = null)
                {
                    _row.Options.Add(new EventOption(code, text, condition));
                    return this;
                }
            }

            public EventDetailCreation RowSelector(Func<Hero, int> func)
            {
                _detail._indexSelector = func;
                _detail._rowSelector= (hero, min, max) =>
                {
                    var index = func(hero);
                    return min <= index && index <= max;
                };
                return this;
            }
        }

        internal class EventRow
        {
            public List<EventOption> Options { get; } = new List<EventOption>();

            public int Min { get; set; }
            public int Max { get; set; }
            public string Text { get; set; }
            public string SubText { get; set; }
        }

        public HeroEvent GetHeroEvent(Hero hero)
        {
            var rows = CreateHeroRows(hero);
            var options = GetHeroEventOptions(hero, null);
            return new HeroEvent
            {
                Name = Name,
                Title = Name,
                Fate = Fate,
                Text = GetText(),
                Rows = rows,
                Options = options,
                IsIgnorable = Name != "Renewal"
            };
        }
    }
}