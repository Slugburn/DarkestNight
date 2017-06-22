using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public static class TacticExtension
    {
        public static List<TacticInfo> GetInfo(this IEnumerable<ITactic> tactics, Hero hero)
        {
            return (from tactic in tactics
                let dice = GetDice(tactic, hero)
                select new TacticInfo
                {
                    Name = tactic.Name,
                    Type = tactic.Type,
                    DiceCount = dice.Total,
                    DiceDetails = dice.Details
                }).ToList();
        }

        public static Dice GetDice(this ITactic tactic, Hero hero)
        {
            var rollType = tactic.Type == TacticType.Fight ? RollType.Fight : RollType.Elude;
            var dice = hero.GetDice(rollType, tactic.Name, tactic.GetDiceCount());
            return dice;
        }
    }
}
