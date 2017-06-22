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
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                var e = hero.CurrentEvent;
                e.Rows.Activate(rollState.Result);
                var result = rollState.Result;
                e.Options.Clear();
                if (result > 3)
                {
                    if (hero.Secrecy > 0)
                        e.Options.Add(new EventOption("secrecy", "Spend Secrecy"));
                    e.Options.Add(EventOption.Continue());
                }
                else
                {
                    if (hero.Grace > 0)
                        e.Options.Add(new EventOption("grace", "Spend Grace"));
                    e.Options.Add(new EventOption("darkness", "+1 Darkness"));
                }
                hero.PresentCurrentEvent();
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.RemoveRollHandler(this);
            }
        }
    }
}
