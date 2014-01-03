using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    abstract class Power : IPower
    {
        public string Name { get; protected set; }
        public PowerType Type { get; protected set; }
        public string Text { get; protected set; }
        public bool StartingPower { get; protected set; }
        public IHero Hero { get; protected set; }
        public bool Active { get; set; }

        protected Power()
        {
            Stash = new Stash();
        }

        public Stash Stash { get; set; }

        public bool Exhausted { get; set; }

        public virtual bool IsUsable()
        {
            return !Exhausted;
        }

        public abstract void Activate();

        public virtual void Deactivate()
        {
            Active = false;
        }

        public void Exhaust()
        {
            Exhausted = true;
            if (Active)
                SuspendEffects(true);
        }

        public void Refresh()
        {
            Exhausted = false;
            if (Active)
                SuspendEffects(false);
        }

        protected virtual void SuspendEffects(bool isSuspended)
        {
            foreach (var effect in Stash.GetPowerEffects())
                effect.Active = !isSuspended;
        }
    }
}