using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class Dice
    {
        private Dice(List<DiceDetail> details)
        {
            Details = details;
        }

        public List<DiceDetail> Details { get; }

        public int Total
        {
            get
            {
                var total = Details.Sum(x => x.Modifier);
                return total > 0 ? total : 1;
            }
        }

        public static Dice Create(Hero hero, RollType rollType, string baseName, int baseDiceCount)
        {
            var baseDetail = new DiceDetail {Name = baseName, Modifier = baseDiceCount};
            var otherDetails = from rollMod in hero.GetRollModifiers()
                let mod = rollMod.GetModifier(hero, rollType)
                where mod != 0
                select new DiceDetail {Name = rollMod.Name, Modifier = mod};
            var details = new[] {baseDetail}.Concat(otherDetails).ToList();
            var dice = new Dice(details);
            return dice;
        }
    }
}