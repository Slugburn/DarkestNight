using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public class ActivateablePower : ActionPower, IActivateable
    {
        public bool IsActive { get; private set; }

        public virtual void Activate(Hero hero)
        {
            IsActive = true;
        }

        public virtual void Deactivate(Hero hero)
        {
            IsActive = false;
        }

        public override void Exhaust(Hero hero)
        {
            if (IsActive)
                Deactivate(hero);
            base.Exhaust(hero);
        }
    }
}
