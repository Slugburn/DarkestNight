using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Pray : IAction
    {
        public string Name => "Pray";
        public void Act(Hero hero)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction && hero.Location == Location.Monastery;
        }
    }
}
