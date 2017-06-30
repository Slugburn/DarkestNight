using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IBonusPower : IPower
    {
    }

    public abstract class BonusPower : Power, IBonusPower
    {
        protected BonusPower()
        {
            Type=PowerType.Bonus;
        }

        public override bool IsUsable(Hero hero)
        {
            // Corruption blight prevents use of bonus powers
            return base.IsUsable(hero) && !hero.IsAffectedByBlight(Blight.Corruption);
        }
    }
}