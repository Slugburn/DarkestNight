using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class LatentSpell : IEventCard, ICallbackHandler
    {
        public EventDetail Detail { get; } = EventDetail.Create("Latent Spell", 2, x => x
            .Text("Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
            .Row(6, "Destroy a blight of your choice anywhere on the board", o => o.Option("destroy-blight", "Continue"))
            .Row(5, "Draw a power card", o => o.Option("draw-power", "Continue"))
            .Row(4, "Move to any other location", r => r.Option("move", "Continue"))
            .Row(1, 3, "No effect", r => r.Option("no-effect", "Continue"))
            .Option("spend-grace", "Spend Grace", hero => hero.CanSpendGrace)
            .Option("discard-event", "Discard Event"));

        public void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "discard-event":
                    hero.LoseSecrecy(1);
                    break;
                case "spend-grace":
                    hero.LoseSecrecy(1);
                    hero.SpendGrace(1);
                    hero.RollEventDice(new EventRollHandler(Detail));
                    return;
                case "destroy-blight":
                    var blights = BlightModel.Create(hero.Game.GetBlights());
                    var selection = new BlightSelectionModel(blights);
                    hero.Player.DisplayBlightSelection(selection, Callback.For(hero, this));
                    break;
                case "draw-power":
                    hero.DrawPower(Callback.For(hero, this));
                    break;
                case "move":
                    var locations = Game.GetAllLocations().Except(new[] {hero.Location}).Select(x=>x.ToString()).ToList();
                    hero.Player.DisplayLocationSelection(locations, Callback.For(hero, this));
                    hero.Player.State = PlayerState.SelectLocation;
                    break;
                case "no-effect":
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.EndEvent();
        }

        public void HandleCallback(Hero hero, object data)
        {
            if (data is Location)
            {
                var location = (Location)data;
                hero.MoveTo(location);
            }
            else if (data is IEnumerable<int>)
            {
                var blightId = ((IEnumerable<int>) data).Single();
                hero.Game.DestroyBlight(hero, blightId);
            }
        }
    }

}
