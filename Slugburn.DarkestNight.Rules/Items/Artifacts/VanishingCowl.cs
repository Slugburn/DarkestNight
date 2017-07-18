using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class VanishingCowl : Artifact, ICommand, ITriggerHandler<Hero>
    {
         
        public VanishingCowl() : base("Vanishing Cowl")
        {
            Text = "You may ignore the effect of one blight each turn.";
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Game.GetBlights().Any();
        }

        public async void Execute(Hero hero)
        {
            var blights = hero.Game.GetBlights();
            var selection = BlightSelectionModel.Create("Ignore Blight [Vanishing Cowl]", blights, 1);
            var blightIds = await hero.SelectBlights(selection);
            var blightId = blightIds.Single();
            hero.Game.AddBlightSupression(new VanishingCowlBlightSupression(Name, hero, blightId));
            hero.Triggers.Add(HeroTrigger.StartedTurn, Name, this);
            hero.ContinueTurn();
        }

        public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
        {
            hero.Game.RemoveBlightSupression(Name);
            return Task.CompletedTask;
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
