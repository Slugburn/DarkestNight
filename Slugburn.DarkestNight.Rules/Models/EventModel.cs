using System.Linq;
using Slugburn.DarkestNight.Rules.Events;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class EventModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Fate { get; set; }
        public EventOptionModel[] Options { get; set; }
        public EventRowModel[] Rows { get; set; }

        public static EventModel From(HeroEvent obj)
        {
            if (obj == null) return null;
            var e = new EventModel
            {
                Title = obj.Title,
                Text = obj.Text,
                Fate = obj.Fate,
                Options = obj.Options.Select(o => new EventOptionModel(o.Code, o.Text)).ToArray()
            };
            if (obj.Rows != null)
                e.Rows = obj.Rows.Select(r=> new EventRowModel
                {
                    Min = r.Min,
                    Max = r.Max,
                    Text = r.Text,
                    SubText = r.SubText,
                    IsActive = r.IsActive
                }).ToArray();
            return e;
        }
    }
}
