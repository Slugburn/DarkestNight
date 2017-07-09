using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public class ActivatePowerAction : PowerAction
    {
        public ActivatePowerAction(IActionPower power) : base(power)
        {
        }

        public override void Execute(Hero hero)
        {
            var power = (IActivateable)Power;
            power.Activate(hero);
            hero.ContinueTurn();
        }
    }
}