using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class CloseCall : IEventCard
    {
        public string Name => "Close Call";
        public int Fate { get; }
        public EventDetail Detail => EventDetail.Create(x=>x.Text("Roll 1 die and take the highest", "5-6: No effect", "3-4: Lose 1 Secrecy", "1-2: Lose 1 Grace").Option("cont", "Continue"));
        public void Resolve(Hero hero, string option)
        {
            var dice = hero.GetDice(RollType.Event, "Event", 1);
            hero.Roll = Die.Roll(dice.Total);
            hero.SetRollHandler(new CloseCallRollHandler());
            hero.State = HeroState.RollAvailable;
        }

        public class CloseCallRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var result = hero.Roll.Max();
                if (result == 5 || result == 6)
                {
                }
                else if (result == 3 || result == 4)
                {
                    hero.LoseSecrecy("Event");
                }
                else if (result == 1 || result == 2)
                {
                    hero.LoseGrace();
                }
                else
                {
                    throw new InvalidOperationException("Unexpected roll.");
                }
                hero.EndEvent();
            }
        }
    }
}
