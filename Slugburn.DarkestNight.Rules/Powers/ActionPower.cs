using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class ActionPower : Power
    {
        protected ActionPower()
        {
            Type = PowerType.Action;
        }

        public override bool IsUsable()
        {
            return base.IsUsable() && Hero.State == HeroState.ChoosingAction;
        }

        public virtual void Deactivate()
        {
        }
    }
}
