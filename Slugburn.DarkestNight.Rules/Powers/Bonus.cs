using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class Bonus : Power
    {
        protected Bonus()
        {
            Type=PowerType.Bonus;
        }

        public override bool IsUsable()
        {
            // Corruption blight prevents use of bonus powers
            return base.IsUsable() && !Hero.GetBlights().Any(x=>x is Corruption);
        }
    }
}