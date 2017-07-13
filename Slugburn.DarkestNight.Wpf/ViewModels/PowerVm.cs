using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class PowerVm
    {
        public static PowerVm Create(PowerModel model)
        {
            return new PowerVm(model);
        }

        public static List<PowerVm> Create(IEnumerable<PowerModel> models)
        {
            return models.Select(Create).ToList();
        }

        private PowerVm(PowerModel model)
        {
            Name = model.Name;
            Decorations = model.IsExhausted ? TextDecorations.Strikethrough : null;
            Style = model.IsActive ? FontStyles.Italic : FontStyles.Normal;
            Card = PowerCardVm.Create(model);
        }

        public PowerVm()
        {
        }

        public string Name { get; set; }
        public PowerCardVm Card { get; set; }

        public TextDecorationCollection Decorations { get; set; }
        public FontStyle Style { get; set; }
    }
}
