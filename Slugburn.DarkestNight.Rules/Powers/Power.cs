using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class Power : IPower
    {
        public string Name { get; protected set; }
        public PowerType Type { get; protected set; }
        public string Text { get; protected set; }
        public string ActiveText { get; protected set; }
        public bool StartingPower { get; protected set; }

        protected Power()
        {
            Stash = new Stash();
        }

        public Stash Stash { get; set; }

        public bool Exhausted { get; set; }

        public virtual bool IsUsable(Hero hero)
        {
            return !Exhausted;
        }

        public virtual void Exhaust(Hero hero)
        {
            Exhausted = true;
        }

        public void Refresh()
        {
            Exhausted = false;
        }

        public virtual void Learn(Hero hero)
        {
        }
    }
}