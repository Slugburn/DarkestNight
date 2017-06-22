using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Pray : IAction
    {
        public string Name => "Pray";
        public void Act(Hero hero)
        {
            hero.State = HeroState.Praying;
            var rollState = hero.SetRoll(RollBuilder.Create<PrayerRoll>().Type(RollType.Pray).Base("Pray", 2).Target(3));
            rollState.Roll();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction && hero.Location == Location.Monastery;
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
            }
        }
    }
}
