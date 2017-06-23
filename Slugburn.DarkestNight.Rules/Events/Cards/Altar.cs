using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Altar : IEventCard
    {
        public EventDetail Detail { get; } =
            EventDetail.Create("Altar", 3,
                x => x.Text("Roll 1d and take the highest")
                    .Row(4, 6, "Pure Altar", "You may spend 1 Secrecy to gain 1 Grace",
                        o => o.Option("spend-secrecy", "Spend Secrecy", h => h.Secrecy > 0).Option("cont", "Continue"))
                    .Row(1, 3, "Defiled Altar", "Spend 1 Grace or +1 Darkness",
                        o => o.Option("spend-grace", "Spend Grace", h => h.Grace > 0).Option("increase-darkness", "+1 Darkness"))
                    .Option("roll", "Roll"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "roll":
                    hero.RollEventDice(new EventRollHandler(Detail));
                    return;
                case "spend-secrecy":
                    hero.SpendSecrecy(1);
                    hero.GainGrace(1, hero.DefaultGrace);
                    break;
                case "spend-grace":
                    hero.SpendGrace(1);
                    break;
                case "increase-darkness":
                    hero.Game.IncreaseDarkness();
                    break;
                case "cont":
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.EndEvent();
        }
    }
}
