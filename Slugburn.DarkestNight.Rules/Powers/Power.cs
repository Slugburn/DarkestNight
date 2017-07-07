using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class Power : IPower
    {
        public string ActiveText { get; protected set; }
        public Hero Owner { get; private set; }
        public string Name { get; protected set; }
        public PowerType Type { get; protected set; }
        public string Text { get; protected set; }
        public abstract string Html { get; }
        public bool StartingPower { get; protected set; }

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

        public void Learn(Hero hero)
        {
            Owner = hero;
            OnLearn();
        }

        protected virtual void OnLearn() {}
    }
}