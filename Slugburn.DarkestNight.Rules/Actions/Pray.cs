using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Pray : StandardAction
    {
        public Pray() : base("Pray")
        {
            Text = "Roll 2d, and gain 1 Grace (up to default) for each die that rolls a 3 or higher. Also refresh your powers.";
        }

        public override void Execute(Hero hero)
        {
            hero.State = HeroState.Praying;
            var rollState = hero.SetRoll(RollBuilder.Create<PrayerRoll>().Type(ModifierType.PrayDice).Base("Pray", 2).Target(3));
            rollState.Roll();
        }

        public override bool IsAvailable(Hero hero)
        {
            return base.IsAvailable(hero)
                   && (hero.Grace < hero.DefaultGrace && hero.CanGainGrace()
                       || hero.Powers.Any(p => p.IsExhausted));
        }

        private class PrayerRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                hero.Player.DisplayPrayer(PrayerModel.From(hero));
                return rollState;
            }

            public async void AcceptRoll(Hero hero, RollState rollState)
            {
                var successes = rollState.Successes;
                hero.GainGrace(successes, hero.DefaultGrace);
                hero.RefreshPowers();
                await hero.Triggers.Send(HeroTrigger.Prayed);
                hero.CurrentRoll = null;
                hero.ContinueTurn();
            }
        }
    }
}
