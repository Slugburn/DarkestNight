using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class TargetModel
    {
        public TargetModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public TargetModel(TargetInfo info)
        {
            Id= info.Id;
            Name = info.Name;
            FightTarget = info.FightTarget;
            EludeTarget = info.EludeTarget;
        }

        public TargetModel(ConflictTarget target)
        {
            Id = target.Id;
            Name = target.Name;
            Target = target.TargetNumber;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int? FightTarget { get; set; }
        public int? EludeTarget { get; set; }
        public int? Target { get; set; }


    }
}