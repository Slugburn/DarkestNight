using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Altar : IEventCard
    {
        private const string PureAltarText = "You may spend 1 Secrecy to gain 1 Grace";
        private const string DefiledAltarText = "Spend 1 Grace or +1 Darkness";
        public string Name => "Altar";
        public int Fate => 3;

        public EventDetail Detail => EventDetail
            .Create(x => x.Text("Roll 1d and take the highest")
                .Row(4, 6, "Pure Altar", PureAltarText)
                .Row(1, 3, "Defiled Altar", DefiledAltarText)
                .Option("roll", "Roll"));
        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    hero.RollEventDice(new AltarRollHandler());
                    var result = hero.Roll.Max();
                    hero.CurrentEvent.Rows.Activate(result);
                    hero.PresentCurrentEvent();
                    break;
                case "secrecy":
                    hero.SpendSecrecy(1);
                    hero.GainGrace(1, hero.DefaultGrace);
                    break;
                case "grace":
                    hero.SpendGrace(1);
                    break;
                case "darkness":
                    hero.Game.IncreaseDarkness();
                    break;
                case "cont":
                    break;
            }
            if (option!="roll")
                hero.EndEvent();
        }

        public class AltarRollHandler : IRollHandler
        {
            public void HandleRoll(Hero hero)
            {
                var e = hero.CurrentEvent;
                var result = hero.Roll.Max();
                e.Options.Clear();
                if (result > 3)
                {
                    e.Title = "Pure Altar";
                    e.Text = PureAltarText;
                    if (hero.Secrecy > 0)
                        e.Options.Add(new EventOption {Code = "secrecy", Text = "Spend Secrecy"});
                    e.Options.Add(EventOption.Continue());
                }
                else
                {
                    e.Title = "Defiled Altar";
                    e.Text = DefiledAltarText;
                    if (hero.Grace > 0)
                        e.Options.Add(new EventOption {Code = "grace", Text = "Spend Grace"});
                    e.Options.Add(new EventOption {Code = "darkness", Text = "+1 Darkness"});
                }
                hero.PresentCurrentEvent();
            }
        }
    }
}
