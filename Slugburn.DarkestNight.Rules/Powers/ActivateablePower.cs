using System;
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
            IsActive = true;
        }

        public virtual bool Deactivate(Hero hero)
        {
            if (!IsActive) return false;
            IsActive = false;
            return true;
        }

        public override string Html => $"<p><b>Action:</b> {Text}</p><p><b>Active:</b> {ActiveText}</p>";
    }
}
