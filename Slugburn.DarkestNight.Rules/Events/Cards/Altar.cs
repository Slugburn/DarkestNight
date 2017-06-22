using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Altar : IEventCard
    {
        public EventDetail Detail
        {
            get
            {
                return EventDetail
                    .Create("Altar", 3,
                    x => x.Text("Roll 1d and take the highest")
                        .Row(4, 6, "Pure Altar", "You may spend 1 Secrecy to gain 1 Grace",
                            o => o.Option("spend-secrecy", "Spend Secrecy", h => h.Secrecy > 0).Option("cont", "Continue"))
                        .Row(1, 3, "Defiled Altar", "Spend 1 Grace or +1 Darkness",
                            o => o.Option("spend-grace", "Spend Grace", h => h.Grace > 0).Option("increase-darkness", "+1 Darkness"))
                        .Option("roll", "Roll"));
            }
        }

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
                        e.Options.Add(new HeroEventOption("secrecy", "Spend Secrecy"));
                    e.Options.Add(HeroEventOption.Continue());
                }
                else
                {
                    if (hero.Grace > 0)
                        e.Options.Add(new HeroEventOption("grace", "Spend Grace"));
                    e.Options.Add(new HeroEventOption("darkness", "+1 Darkness"));
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
