using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Power
    {
        public static Power Create(PowerModel model)
        {
            return new Power(model);
        }

        public static List<Power> Create(IEnumerable<PowerModel> models)
        {
            return models.Select(Create).ToList();
        }

        private Power(PowerModel model)
        {
            Name = model.Name;
            Text = model.Text;
            Decorations = model.IsExhausted ? TextDecorations.Strikethrough : null;
            Style = model.IsActive ? FontStyles.Italic : FontStyles.Normal;
        }

        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public bool IsExhausted { get; set; }
        public TextDecorationCollection Decorations { get; set; }
        public FontStyle Style { get; set; }
    }
}
