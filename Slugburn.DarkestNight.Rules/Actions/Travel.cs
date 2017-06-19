using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Travel : IAction
    {
        public string Name => "Travel";
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
