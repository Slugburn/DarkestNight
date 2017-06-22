using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class TwistOfFate : IEventCard
    {
        private const string EventName = "Twist of Fate";

        public EventDetail Detail => EventDetail.Create("Twist of Fate", 0, x => x
            .Text("Roll 1d and take the highest")
            .Row(5, 6, "+1d on all rolls for the rest of this turn")
            .Row(1, 4, "-1d (to a minimum of 1d) on all rolls for the rest of this turn")
            .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "roll")
            {
                hero.RollEventDice(new TwistOfFateRollHandler());
                return;
            }
        }

        public class TwistOfFateRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var result = hero.CurrentRoll.Result;
                var dieCount = result >= 5 ? 1 : -1;
                hero.AddRollModifier(new StaticRollBonus {Name = EventName, RollType = RollType.Any, DieCount = dieCount});
                hero.Triggers.Add(HeroTrigger.EndOfTurn, EventName, new TwistOfFateEndOfTurnHandler() );
            }

            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var e = hero.CurrentEvent;
                e.Rows.Activate(rollState.Result);
                throw new NotImplementedException();
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                throw new NotImplementedException();
            }

            public class TwistOfFateEndOfTurnHandler : ITriggerHandler<Hero>
            {
                public void HandleTrigger(Hero registrar, string source, TriggerContext context)
                {
                    registrar.RemoveRollModifiers(EventName);
                    registrar.Triggers.Remove(HeroTrigger.EndOfTurn, EventName);
                }

            }
        }
    }
}
