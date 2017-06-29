using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerAction : IAction
    {
        protected readonly IPower _power;

        protected PowerAction(IPower power)
        {
            _power = power;
            Name = power.Name;
            Text = power.Text;
        }

        protected PowerAction(string name, IPower power)
        {
            _power = power;
            Name = name;
            Text = power.Text;
        }

        public string Name { get; protected set; }

        public string Text { get; protected set; }

        public abstract void Act(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return _power.IsUsable(hero);
        }
    }
}