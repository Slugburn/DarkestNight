using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Celerity : ActionPower
    {
        private const string PowerName = "Celerity";

        public Celerity()
        {
            Name = PowerName;
            Text = "Deactivate all Forms. Travel. Optionally activate one of your Forms.";
        }

        protected override void OnLearn()
        {
            Owner.AddAction(new CelerityAction(this));
        }

        private class CelerityAction : PowerAction, ICallbackHandler<Location>
        {
            public CelerityAction(IActionPower power) : base(power)
            {
            }

            public override void Execute(Hero hero)
            {
                DruidFormPower.DeactivateAllForms(hero);
                hero.GainSecrecy(1, 5);
                var validDestinations = hero.GetValidMovementLocations().Select(x=>x.ToString()).ToList();
                hero.State = HeroState.Moving;
                hero.SelectLocation(validDestinations, this);
            }

            public void HandleCallback(Hero hero, Location data)
            {
                // Move to selected location
                var destination = data;
                hero.MoveTo(destination);
                hero.State = HeroState.FinishedMoving;

                // Allow player to pick a new form
                hero.AddFreeAction(new CelerityActionFilter());
                hero.ContinueTurn();
            }

            private class CelerityActionFilter : IActionFilter
            {
                public string Name => PowerName;
                public bool IsAllowed(ICommand command)
                {
                    var powerCommand = command as PowerCommand;
                    return powerCommand?.Power is IDruidForm;
                }
            }
        }
    }
}