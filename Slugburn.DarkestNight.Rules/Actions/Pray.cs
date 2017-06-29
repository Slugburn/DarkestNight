using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Pray : IAction
    {
        public string Name { get; protected set; } = "Pray";

        public string Text => @"Roll 2d, and gain 1 Grace (up to default) for each die that rolls a 3 or higher. Also refresh your powers.";

        public void Act(Hero hero)
        {
            hero.IsActionAvailable = false;
            var rollState = hero.SetRoll(RollBuilder.Create<PrayerRoll>().Type(RollType.Pray).Base("Pray", 2).Target(3));
            rollState.Roll();
            hero.Player.DisplayPrayer(PlayerPrayer.From(hero));
        }

        public virtual bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn
                   && hero.IsActionAvailable
                   && hero.Location == Location.Monastery
                   && (hero.Grace < hero.DefaultGrace || hero.Powers.Any(p => p.Exhausted));
        }

        private class PrayerRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                var successes = rollState.Successes;
                hero.GainGrace(successes, hero.DefaultGrace);
                hero.RefreshPowers();
                hero.Triggers.Send(HeroTrigger.Prayed);
            }
        }
    }
}
