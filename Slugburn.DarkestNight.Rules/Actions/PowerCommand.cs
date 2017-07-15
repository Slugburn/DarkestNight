using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerCommand : ICommand
    {
        public IPower Power { get; }

        protected PowerCommand(IPower power, bool requiresAction)
        {
            Power = power;
            RequiresAction = requiresAction;
            Name = power.Name;
            Text = power.Text;
        }

        protected PowerCommand(string name, IPower power, bool requiresAction)
        {
            Power = power;
            RequiresAction = requiresAction;
            Name = name;
            Text = power.Text;
        }

        public string Name { get; protected set; }

        public string Text { get; protected set; }
        public bool RequiresAction { get; }

        public abstract void Execute(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return Power.IsUsable(hero);
        }
    }
}