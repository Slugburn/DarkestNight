using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    abstract class Action : Power
    {
        protected Action()
        {
            Type = PowerType.Action;
        }

        public string ActiveText { get; protected set; }

        public override bool IsUsable()
        {
            return base.IsUsable() && Hero.State == HeroState.ChoosingAction;
        }
    }
}
