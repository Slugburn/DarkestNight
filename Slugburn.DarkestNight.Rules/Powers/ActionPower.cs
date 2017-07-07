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
                   && hero.State == HeroState.TakingTurn
                   && hero.IsTakingTurn
                   && hero.IsActionAvailable
                   && hero.ConflictState == null
                   && hero.CurrentEvent == null;
        }

        public override string Html => $"<p><b>Action:</b> {Text}</p>";
    }
}
