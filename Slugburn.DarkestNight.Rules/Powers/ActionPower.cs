using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class ActionPower : Power, IAction
    {
        protected ActionPower()
        {
            Type = PowerType.Action;
        }

        public override bool IsUsable()
        {
            return base.IsUsable() && Hero.State == HeroState.ChoosingAction;
        }

        public bool Act()
        {
            if (!IsUsable())
                throw new PowerNotUsableException(this);
            return TakeAction();
        }

        public virtual void Deactivate()
        {
        }

        protected abstract bool TakeAction();
    }
}
