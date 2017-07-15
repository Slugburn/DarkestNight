using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Powers
{
    internal abstract class ActivateOnSpacePower : ActivateablePower, ITargetable
    {
        protected Space Target { get; set; }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            if (Target == null)
            {
                PayActivationCost();
                Target = hero.Space;
            }
            ActivateEffect();
        }

        protected abstract void PayActivationCost();
        protected abstract void ActivateEffect();

        public void SetTarget(string targetName)
        {
            var location = targetName.ToEnum<Location>();
            Target = Owner.Game.Board[location];
        }

        public string GetTarget()
        {
            return Target.Location.ToString();
        }
    }
}