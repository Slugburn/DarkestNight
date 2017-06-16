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
        public Hero Hero { get; internal set; }
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

        public void Exhaust()
        {
            Exhausted = true;
        }

        public void Refresh()
        {
            Exhausted = false;
        }

        public virtual void Learn()
        {
        }
    }
}