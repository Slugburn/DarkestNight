using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class VanishingCowl : Artifact, ICommand, ICallbackHandler, ITriggerHandler<Hero>
    {
         
        public VanishingCowl() : base("Vanishing Cowl")
        {
            Text = "You may ignore the effect of one blight each turn.";
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Game.GetBlights().Any();
        }

        public void Execute(Hero hero)
        {
            var playerBlights = PlayerBlight.Create(hero.Game.GetBlights());
            hero.Player.DisplayBlightSelection(new PlayerBlightSelection(playerBlights), Callback.ForCommand(hero, this));
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            var blightId = ((IEnumerable<int>)data).Single();
            hero.Game.AddBlightSupression(new VanishingCowlBlightSupression(Name, hero, blightId));
            hero.Triggers.Add(HeroTrigger.StartedTurn, Name, this);
            hero.ContinueTurn();
        }

        public void HandleTrigger(Hero hero, string source, TriggerContext context)
        {
            hero.Game.RemoveBlightSupression(Name);
        }

        internal class VanishingCowlBlightSupression : IBlightSupression
        {
            private readonly Hero _hero;
            private readonly int _blightId;

            public VanishingCowlBlightSupression(string name, Hero hero, int blightId)
            {
                Name = name;
                _hero = hero;
                _blightId = blightId;
            }

            public string Name { get; }
            public bool IsSupressed(IBlight blight, Hero hero = null)
            {
                return hero == _hero && blight.Id == _blightId;
            }
        }
    }
}
