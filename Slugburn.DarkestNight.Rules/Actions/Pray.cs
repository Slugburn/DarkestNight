using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Pray : IAction
    {
        public string Name => "Pray";
        public void Act(Hero hero)
        {
            hero.State = HeroState.Praying;
            var dice = hero.GetDice(RollType.Pray, "Pray", 2);
            hero.Roll = Die.Roll(dice.Total);
            hero.Triggers.Handle(HeroTrigger.AfterRoll);
            var successes = hero.Roll.Count(x => x>=3);
            hero.GainGrace(successes, hero.DefaultGrace);
            hero.RefreshPowers();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction && hero.Location == Location.Monastery;
        }
    }
}
