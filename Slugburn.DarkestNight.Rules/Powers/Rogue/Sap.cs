using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Sap : BonusPower
    {
        public Sap()
        {
            Name = "Sap";
            Text = "Exhaust during your turn to reduce the might of a blight in your location by 1 until your next turn.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.GetBlights().Any();
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new SapCommand(this));
        }

        private class SapCommand : PowerCommand
        {
            public SapCommand(IPower power) : base(power, false) { }

            public override async void Execute(Hero hero)
            {
                Power.Exhaust(hero);
                var blightIds = await hero.SelectBlights(BlightSelectionModel.Create("Sap", hero.Space.Blights, 1));
                var blightId = blightIds.Single();
                var blight = hero.Game.GetBlight(blightId);
                blight.ReduceMight();
                hero.Game.UpdatePlayerBoard();
                hero.Triggers.Add(HeroTrigger.StartedTurn, Name, new SapOnStartedTurn(blight));
            }
        }

        private class SapOnStartedTurn : ITriggerHandler<Hero>
        {
            private readonly IBlight _blight;

            public SapOnStartedTurn(IBlight blight)
            {
                _blight = blight;
            }

            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                _blight.RestoreMight();
                hero.Game.UpdatePlayerBoard();
            }
        }
    }
}