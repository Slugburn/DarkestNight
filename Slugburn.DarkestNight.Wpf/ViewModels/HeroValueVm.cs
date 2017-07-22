using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroValueVm
    {
        public static HeroValueVm Create(HeroValueModel model)
        {
            return new HeroValueVm
            {
                Text = model.Name,
                Value = model.Value,
                Default = model.Default
            };
        }

        public string Text { get; set; }
        public int Default { get; set; }
        public int Value { get; set; }
    }
}