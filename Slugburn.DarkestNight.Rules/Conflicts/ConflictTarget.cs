using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Conflicts
{
    public class ConflictTarget
    {
        public TacticType TacticType { get; }
        private bool _ignoreFailure;

        public ConflictTarget(IConflict conflict, TargetInfo targetInfo, TacticType tacticType)
        {
            TacticType = tacticType;
            Conflict = conflict;
            Id = targetInfo.Id;
            Name = conflict.Name;
            TargetNumber = tacticType == TacticType.Fight ? targetInfo.FightTarget : targetInfo.EludeTarget;
        }

        public IConflict Conflict { get; }

        public int Id { get; }
        public string Name { get; }
        public int TargetNumber { get; }
        public int? ResultNumber { get; set; }

        public bool IsWin => ResultNumber >= TargetNumber;

        public void Resolve(Hero hero)
        {
            if (IsWin)
            {
                Conflict.Win(hero);
            }
            else if (!_ignoreFailure)
            {
                Conflict.Failure(hero);
            }
        }

        public void IgnoreFailure()
        {
            _ignoreFailure = true;
        }
    }
}