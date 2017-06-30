using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Conflicts
{
    public class ConflictState
    {
        public ConflictType ConflictType { get; set; }
        public int MinTarget { get; set; }
        public int MaxTarget { get; set; }
        public ICollection<ConflictTarget> SelectedTargets { get; set; }
        public TacticInfo SelectedTactic { get; set; }
        public List<TacticInfo> AvailableTactics { get; set; }
        public List<TacticInfo> AvailableEvadeTactics { get; set; }
        public ICollection<TargetInfo> AvailableTargets { get; set; }
        public ICollection<int> Roll { get; set; }
        public ICollection<ConflictTarget> Effects {get;} = new List<ConflictTarget>();

        public void Resolve(Hero hero)
        {
            var rollState = hero.CurrentRoll;
            var result = rollState.Result;
            var target = SelectedTargets.Single();
            var assignment = TargetDieAssignment.Create(target.Id, result);
            hero.AssignDiceToTargets(new[] {assignment});
        }

        public void Display(IPlayer player)
        {
            player.DisplayConflict(PlayerConflict.FromConflictState(this));
            player.State = PlayerState.Conflict;
        }

        public TacticType GetTacticType()
        {
            return SelectedTactic.Type;
        }
    }
}