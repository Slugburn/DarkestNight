using System.Collections;
using System.Linq;
using Slugburn.DarkestNight.Rules.Events;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerEvent
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Fate { get; set; }
        public Option[] Options { get; set; }
        public Row[] Rows { get; set; }

        public class Row
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public string Text { get; set; }
            public bool IsActive { get; set; }
        }

        public class Option
        {
            public string Code { get; set; }
            public string Text { get; set; }
        }

        public static PlayerEvent From(HeroEvent obj)
        {
            var e = new PlayerEvent
            {
                Title = obj.Title,
                Text = obj.Text,
                Fate = obj.Fate,
                Options = obj.Options.Select(o => new Option {Code = o.Code, Text = o.Text}).ToArray()
            };
            if (obj.Rows != null)
                e.Rows = obj.Rows.Select(r=> new Row { Min = r.Min, Max = r.Max, Text = r.Text, IsActive = r.IsActive}).ToArray();
            return e;
        }
    }
}
