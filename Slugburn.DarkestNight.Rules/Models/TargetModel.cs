using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class TargetModel
    {
        public TargetModel(TargetInfo info)
        {
            Id = info.Id;
            Name = info.Name;
            CanFight = info.CanFight;
            FightTarget = info.FightTarget;
            CanElude = info.CanElude;
            EludeTarget = info.EludeTarget;
        }

        public TargetModel(ConflictTarget target)
        {
            Id = target.Id;
            Name = target.Name;
            TargetNumber = target.TargetNumber;
            Result = target.ResultNumber;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public bool? CanFight { get; set; }
        public int? FightTarget { get; set; }
        public bool? CanElude { get; set; }
        public int? EludeTarget { get; set; }
        public int? TargetNumber { get; set; }
        public int? Result { get; set; }
    }
}