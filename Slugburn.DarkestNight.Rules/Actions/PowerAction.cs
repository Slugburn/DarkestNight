using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerAction : IAction
    {
        protected readonly string _powerName;

        protected PowerAction(string name)
        {
            Name = name;
            _powerName = name;
        }

        protected PowerAction(string name, string powerName)
        {
            Name = name;
            _powerName = powerName;
        }

        public string Name { get; }

        public abstract void Act(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return hero.GetPower(_powerName).IsUsable(hero);
        }
    }
}