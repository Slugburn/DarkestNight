using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

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

        private static Dice GetDice(ITactic tactic, Hero hero)
        {
            var baseDetail = new DiceDetail {Name = tactic.Name, Modifier = tactic.GetDiceCount()};
            var otherDetails = from rollMod in hero.GetRollModifiers()
                let mod = rollMod.GetModifier(hero)
                where mod != 0
                select new DiceDetail {Name = rollMod.Name, Modifier = mod};
            var details = new[] {baseDetail}.Concat(otherDetails).ToList();
            var dice = new Dice(details);
            return dice;
        }
    }
}
