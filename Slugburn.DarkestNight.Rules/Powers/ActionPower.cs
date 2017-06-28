using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class ActionPower : Power
    {
        protected ActionPower()
        {
            Type = PowerType.Action;
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.IsActing && hero.IsActionAvailable;
        }
    }
}
