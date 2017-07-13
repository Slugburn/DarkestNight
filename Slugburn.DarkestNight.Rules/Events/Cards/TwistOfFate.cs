using System;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class TwistOfFate : IEventCard
    {
        private const string EventName = "Twist of Fate";

        public EventDetail Detail => EventDetail.Create("Twist of Fate", 1, x => x
            .Text("Roll 1d and take the highest")
            .Row(5, 6, "+1d on all rolls for the rest of this turn", o=>o.Option("plus-die", "Continue"))
            .Row(1, 4, "-1d (to a minimum of 1d) on all rolls for the rest of this turn", o=>o.Option("minus-die", "Continue"))
            .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            if (option == "roll")
            {
                hero.RollEventDice(new EventRollHandler(Detail));
                return;
            }
            else if (option == "plus-die")
            {
                hero.AddModifier(StaticRollBonus.AnyRoll(EventName, 1));
            }
            else if (option == "minus-die")
            {
                hero.AddModifier(StaticRollBonus.AnyRoll(EventName, -1));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.Triggers.Add(HeroTrigger.TurnEnded, EventName, new TwistOfFateEndOfTurnHandler());
            hero.EndEvent();
        }

        public class TwistOfFateEndOfTurnHandler : ITriggerHandler<Hero>
        {
            public void HandleTrigger(Hero registrar, string source, TriggerContext context)
            {
                registrar.RemoveRollModifiers(EventName);
                registrar.Triggers.Remove(HeroTrigger.TurnEnded, EventName);
            }

        }
    }
}
