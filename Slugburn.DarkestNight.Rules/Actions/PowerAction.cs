using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerCommand : ICommand
    {
        protected readonly IPower _power;

        protected PowerCommand(IPower power)
        {
            _power = power;
            Name = power.Name;
            Text = power.Text;
        }

        protected PowerCommand(string name, IPower power)
        {
            _power = power;
            Name = name;
            Text = power.Text;
        }

        public string Name { get; protected set; }

        public string Text { get; protected set; }

        public abstract void Execute(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return _power.IsUsable(hero);
        }
    }

    public abstract class PowerAction : PowerCommand, IAction
    {
        protected PowerAction(IActionPower power) : base(power)
        {
        }

        protected PowerAction(string name, IActionPower power) : base(name, power)
        {
        }
    }
}