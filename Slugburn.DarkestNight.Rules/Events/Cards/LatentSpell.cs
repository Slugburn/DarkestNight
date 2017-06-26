using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class LatentSpell : IEventCard
    {
        public EventDetail Detail { get; } = EventDetail.Create("Latent Spell", 2, x => x
            .Text("Lose 1 Secrecy. Then, spend 1 Grace or discard this event without further effect.\nRoll 1d and take the highest")
            .Row(6, "Destroy a blight of your choice anywhere on the board", o => o.Option("destroy-blight", "Continue"))
            .Row(5, "Draw a power card", o => o.Option("draw-power", "Continue"))
            .Row(4, "Move to any other location", r => r.Option("move", "Continue"))
            .Row(1, 3, "No effect", r => r.Option("no-effect", "Continue"))
            .Option("spend-grace", "Spend Grace", hero => hero.Grace > 0)
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
                    hero.Game.Triggers.Add(GameTrigger.PlayerSelectedBlight, Detail.Name, new LatentSpellBlightSelected() );
                    var blights = hero.Game.Board.Spaces.SelectMany(s => s.Blights.Select(b => new PlayerBlight {Location = s.Location, Blight = b})).ToList();
                    var selection = new PlayerBlightSelection(blights);
                    hero.Player.DisplayBlightSelection(selection);
                    break;
                case "draw-power":
                    var power = hero.PowerDeck.First();
                    hero.LearnPower(power.Name);
                    hero.Player.DisplayPowers(new [] {power}.ToPlayerPowers());
                    hero.Player.State = PlayerState.SelectPower;
                    break;
                case "move":
                    var locations = Game.GetAllLocations().Except(new[] {hero.Location}).Select(x=>x.ToString()).ToList();
                    hero.SetLocationSelectedHandler(new LatentSpellSelectLocation());
                    hero.Player.DisplayLocationSelection(locations);
                    hero.Player.State = PlayerState.SelectLocation;
                    break;
                case "no-effect":
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
            hero.EndEvent();
        }

        private class LatentSpellBlightSelected : ITriggerHandler<Game>
        {
            public void HandleTrigger(Game game, string source, TriggerContext context)
            {
                var blightSelection = context.GetState<BlightLocation>();
                game.DestroyBlight(blightSelection.Location, blightSelection.Blight);
                game.Triggers.RemoveBySource(source);
            }
        }

        private class LatentSpellSelectLocation : ILocationSelectedHandler
        {
            public void Handle(Hero hero, Location location)
            {
                hero.MoveTo(location);
            }
        }
    }

}
