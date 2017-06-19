using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class TwistOfFate : IEventCard
    {
        private const string EventName = "Twist of Fate";
        public string Name => EventName;

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Roll 1 die and take the highest", "5-6: +1 die on all rolls for the rest of this turn", "1-4: -1 die (to a minimum of 1 die) on all rolls for the rest of this turn"));

        public void Resolve(Hero hero, string option)
        {
            hero.RollEventDice(new TwistOfFateRollHandler());
        }

        public class TwistOfFateRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var roll = hero.Roll;
                var result = roll.Max();
                var dieCount = result >= 5 ? 1 : -1;
                hero.AddRollModifier(new StaticRollBonus {Name = EventName, RollType = RollType.Any, DieCount = dieCount});
                hero.Triggers.Register(HeroTrigger.EndOfTurn, new TwistOfFateEndOfTurnHandler() );
            }

            public class TwistOfFateEndOfTurnHandler : ITriggerHandler<Hero>
            {
                public string Name => EventName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    registrar.RemoveRollModifier(EventName);
                    registrar.Triggers.Unregister(HeroTrigger.EndOfTurn, EventName);
                }

            }
        }
    }
}
