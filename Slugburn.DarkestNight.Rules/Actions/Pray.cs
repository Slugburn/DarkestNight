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
            hero.Roll = RollState.Create(Die.Roll(dice.Total));
            hero.Triggers.Send(HeroTrigger.AfterRoll);
            hero.Roll.TargetNumber = 3;
            var successes = hero.Roll.Successes;
            hero.GainGrace(successes, hero.DefaultGrace);
            hero.RefreshPowers();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction && hero.Location == Location.Monastery;
        }
    }
}
