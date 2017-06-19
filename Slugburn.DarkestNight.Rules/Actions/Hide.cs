using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Hide : IAction
    {
        public string Name => "Hide";

        public void Act(Hero hero)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction;
        }
    }
}
