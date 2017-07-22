using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class EffectModel
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static List<EffectModel> Create(IEnumerable<Effect> effects)
        {
            return effects.Select(Create).ToList();
        }

        public static EffectModel Create(Effect effect)
        {
            return new EffectModel {Name = effect.Name, Text = effect.Text};
        }
    }
}