using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroPowerVm
    {
        public static HeroPowerVm Create(PowerModel model)
        {
            return new HeroPowerVm(model);
        }

        public static List<HeroPowerVm> Create(IEnumerable<PowerModel> models)
        {
            return models.Select(Create).ToList();
        }

        private HeroPowerVm(PowerModel model)
        {
            Name = model.Name;
            Decorations = model.IsExhausted ? TextDecorations.Strikethrough : null;
            Style = model.IsActive ? FontStyles.Italic : FontStyles.Normal;
            Card = PowerCardVm.Create(model);
        }

        public string Name { get; set; }
        public PowerCardVm Card { get; set; }

        public TextDecorationCollection Decorations { get; set; }
        public FontStyle Style { get; set; }
    }
}
