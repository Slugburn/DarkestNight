﻿using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Altar : IEvent
    {
        public string Name => "Altar";
        public EventDetail Detail => EventDetail.Create(x=>x.Text("Roll 1 die and take the highest.","4-6: Pure Altar","1-3: Defiled Altar").Option("roll", "Roll Die"));
        public void Resolve(Hero hero, string option)
        {
            hero.RollEventDice(new AltarRollHandler());
        }

        public class AltarRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var result = hero.Roll.Max();
                var subEvent = result > 3 ? (IEvent)new PureAltar() : new DefiledAltar();
                hero.PresentEvent(subEvent);
            }
        }

        public class PureAltar : IEvent
        {
            public string Name { get; }
            public EventDetail Detail { get; }
            public void Resolve(Hero hero, string option)
            {
                throw new System.NotImplementedException();
            }
        }

        public class DefiledAltar : IEvent
        {
            public string Name { get; }
            public EventDetail Detail { get; }
            public void Resolve(Hero hero, string option)
            {
                throw new System.NotImplementedException();
            }
        }

    }
}
