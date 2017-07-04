using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Target
    {
        public Target(TargetModel model)
        {
            Id = model.Id;
            Name = model.Name;
            TargetNumber = model.Target?.ToString() ?? $"Fight {model.FightTarget} / Elude {model.EludeTarget}";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TargetNumber { get; set; }
    }
}