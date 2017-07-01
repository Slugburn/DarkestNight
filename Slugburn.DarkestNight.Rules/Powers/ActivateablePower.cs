using System;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class ActivateablePower : ActionPower, IActivateable
    {
        public bool IsActive { get; private set; }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && !IsActive;
        }

        protected override void OnLearn()
        {
            Owner.AddAction(new ActivatePowerAction(this));
        }

        public virtual void Activate(Hero hero)
        {
            if (!IsUsable(hero))
                throw new InvalidOperationException($"{Name} is not usable.");
            IsActive = true;
        }

        public virtual bool Deactivate(Hero hero)
        {
            if (!IsActive) return false;
            IsActive = false;
            return true;
        }

        public override void Exhaust(Hero hero)
        {
            if (IsActive)
                Deactivate(hero);
            base.Exhaust(hero);
        }

        public class ActivatePowerAction : PowerAction
        {
            public ActivatePowerAction(IActionPower power) : base(power)
            {
            }

            public override void Execute(Hero hero)
            {
                var power = (IActivateable)_power;
                power.Activate(hero);
            }
        }
    }
}
