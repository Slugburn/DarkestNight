using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroStatusVm
    {
        public string Location { get; set; }
        public HeroValue Grace { get; set; }
        public HeroValue Secrecy { get; set; }

        public static HeroStatusVm Create(HeroStatusModel model)
        {
            return new HeroStatusVm
            {
                Location = model.Location,
                Grace = new HeroValue(model.Grace),
                Secrecy = new HeroValue(model.Secrecy)
            };
        }
    }
}
