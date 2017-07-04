using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroValue
    {
        public HeroValue(HeroValueModel model)
        {
            Name = model.Name;
            Value = model.Value;
            Default = model.Default;
        }

        public int Default { get; set; }

        public int Value { get; set; }

        public string Name { get; set; }
    }
}