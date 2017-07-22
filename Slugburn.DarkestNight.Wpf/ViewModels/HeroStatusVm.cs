using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroStatusVm
    {
        public string Location { get; set; }
        public HeroValueVm Grace { get; set; }
        public HeroValueVm Secrecy { get; set; }
        public bool CanStartTurn { get; set; }
        public bool HasTakenTurn { get; set; }

        public static HeroStatusVm Create(HeroStatusModel model)
        {
            return new HeroStatusVm
            {
                Location = model.Location,
                Grace = HeroValueVm.Create(model.Grace),
                Secrecy = HeroValueVm.Create(model.Secrecy),
                CanStartTurn = model.CanStartTurn,
                HasTakenTurn = model.HasTakenTurn
            };
        }
    }
}
