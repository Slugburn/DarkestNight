using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class Bonus : Power
    {
        protected Bonus()
        {
            Type=PowerType.Bonus;
        }

        public override bool IsUsable(Hero hero)
        {
            // Corruption blight prevents use of bonus powers
            return base.IsUsable(hero) && hero.GetBlights().All(x => x != Blight.Corruption);
        }
    }
}