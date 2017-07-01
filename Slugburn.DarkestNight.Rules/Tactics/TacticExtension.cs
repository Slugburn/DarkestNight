using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public static class TacticExtension
    {
        public static List<TacticInfo> GetInfo(this IEnumerable<ITactic> tactics, Hero hero)
        {
            return (from tactic in tactics
                let summary = GetSummary(tactic, hero)
                select new TacticInfo
                {
                    Name = tactic.Name,
                    Type = tactic.Type,
                    DiceCount = summary.Total,
                    DiceDetails = summary.Details
                }).ToList();
        }

        public static ModifierSummary GetSummary(this ITactic tactic, Hero hero)
        {
            var rollType = GetRollType(tactic);
            var summary = hero.CreateModifierSummary(rollType, tactic.Name, tactic.GetDiceCount());
            return summary;
        }

        public static ModifierType GetRollType(this ITactic tactic)
        {
            return tactic.Type == TacticType.Fight ? ModifierType.FightDice : ModifierType.EludeDice;
        }
    }
}
