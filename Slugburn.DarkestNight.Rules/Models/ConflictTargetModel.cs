using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class ConflictTargetModel
    {
        public ConflictTargetModel(ConflictTarget target)
        {
            Id = target.Id;
            Name = target.Name;
            TacticType = target.TacticType.ToString();
            TargetNumber = target.TargetNumber;
            ResultNumber = target.ResultDie;
            IsWin = target.IsWin;
            OutcomeDescription = target.Conflict.OutcomeDescription(target.IsWin, target.TacticType);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TacticType { get; set; }
        public int TargetNumber { get; set; }
        public int ResultNumber { get; set; }
        public bool IsWin { get; set; }
        public string OutcomeDescription { get; set; }
    }
}