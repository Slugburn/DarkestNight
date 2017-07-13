using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroStatus
    {
        public HeroStatus(HeroStatusModel model)
        {
            Grace = new HeroValue(model.Grace);
            Secrecy = new HeroValue(model.Secrecy);
        }

        public HeroStatus()
        {
        }

        public HeroValue Grace { get; set; }
        public HeroValue Secrecy { get; set; }
    }
}
