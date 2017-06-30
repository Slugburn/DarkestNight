using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface ITacticPower : IPower
    {
    }

    public abstract class TacticPower : Power, ITacticPower
    {
        protected TacticPower()
        {
            Type = PowerType.Tactic;
        }

        public override bool IsUsable(Hero hero)
        {
            // Confusion blight prevents use of tactic powers
            return base.IsUsable(hero) && !hero.IsAffectedByBlight(BlightType.Confusion);
        }
    }
}
