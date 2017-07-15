using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerAction : PowerCommand, IAction
    {
        protected PowerAction(IActionPower power) : base(power, true)
        {
        }

        protected PowerAction(string name, IActionPower power) : base(name, power, true)
        {
        }
    }
}