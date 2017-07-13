﻿using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class EvilDay : IEventCard, ICallbackHandler<string>
    {
        public EventDetail Detail => EventDetail.Create("Evil Day", 5,
            x => x
                .Text("Exhaust a power or draw 2 more events.")
                .Option("exhaust", "Exhaust Power", hero => hero.Powers.Any(power => !power.Exhausted))
                .Option("draw", "Draw Events"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "exhaust":
                    var powers = hero.Powers.Where(x => !x.Exhausted).Select(PowerModel.Create).ToList();
                    var player = hero.Player;
                    player.State = PlayerState.SelectPower;
                    player.DisplayPowers(powers, Callback.For<string>(hero, this));
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

        public void HandleCallback(Hero hero, string data)
        {
            var powerName = data;
            var power = hero.GetPower(powerName);
            power.Exhaust(hero);
            hero.EndEvent();
        }
    }
}
