using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Altar : IEventCard
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
                var subEvent = result > 3 ? (IEventCard)new PureAltar() : new DefiledAltar();
                hero.PresentEvent(subEvent);
            }
        }

        public class PureAltar : IEventCard
        {
            public string Name { get; }
            public EventDetail Detail { get; }
            public void Resolve(Hero hero, string option)
            {
                throw new System.NotImplementedException();
            }
        }

        public class DefiledAltar : IEventCard
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
