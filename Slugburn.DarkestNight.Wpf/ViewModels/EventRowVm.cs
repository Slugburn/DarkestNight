using System.Windows.Media;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class EventRowVm
    {
        private EventRowVm(EventRowModel model)
        {
            Text = model.Text;
            SubText = model.SubText;
            if (model.Max == int.MaxValue)
                Range = $"{model.Min}+";
            else if (model.Min == model.Max)
                Range = model.Min.ToString();
            else
                Range = $"{model.Min}-{model.Max}";
            Highlight = model.IsActive ? new SolidColorBrush(Colors.Yellow) : null;
        }

        public EventRowVm()
        {
        }

        public static EventRowVm Create(EventRowModel model)
        {
            return new EventRowVm(model);
        }

        public string Range { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public Brush Highlight { get; set; }
    }
}