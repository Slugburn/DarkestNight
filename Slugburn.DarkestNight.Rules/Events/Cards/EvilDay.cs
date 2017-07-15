using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class EvilDay : IEventCard
    {
        public EventDetail Detail => EventDetail.Create("Evil Day", 5,
            x => x
                .Text("Exhaust a power or draw 2 more events.")
                .Option("exhaust", "Exhaust Power", hero => hero.Powers.Any(power => !power.IsExhausted))
                .Option("draw", "Draw Events"));

        public async void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "exhaust":
                    var powers = hero.Powers.Where(x => !x.IsExhausted).Select(PowerModel.Create).ToList();
                    var player = hero.Player;
                    player.State = PlayerState.SelectPower;
                    var selectedName = await player.SelectPower(powers);
                    var power = hero.GetPower(selectedName);
                    power.Exhaust(hero);
                    hero.EndEvent();
                    break;
                case "draw":
                    var newEvents = hero.Game.Events.Draw(2);
                    foreach (var e in newEvents)
                    {
                        hero.EventQueue.Enqueue(e);
                    }
                    hero.EndEvent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
        }
    }
}
