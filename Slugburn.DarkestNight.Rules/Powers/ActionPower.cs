using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IActionPower : IPower
    {
    }

    public abstract class ActionPower : Power, IActionPower
    {
        protected ActionPower()
        {
            Type = PowerType.Action;
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero)
                   && hero.IsTakingTurn
                   && hero.IsActionAvailable
                   && hero.ConflictState == null
                   && hero.CurrentEvent == null;
        }
    }
}
