using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class EffectVm
    {
        public string Tooltip { get; set; }
        public string Description { get; set; }

        public static List<EffectVm> Create(List<EffectModel> models)
        {
            return models.Select(Create).ToList();
        }

        public static EffectVm Create(EffectModel model)
        {
            return new EffectVm {Description = model.Name, Tooltip = model.Text};
        }
    }
}