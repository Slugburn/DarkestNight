using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

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
            Owner.AddCommand(new CelerityAction(this));
        }

        private class CelerityAction : PowerAction
        {
            public CelerityAction(IActionPower power) : base(power)
            {
            }

            public override async Task ExecuteAsync(Hero hero)
            {
                DruidFormPower.DeactivateAllForms(hero);
                hero.GainSecrecy(1, 5);
                var validDestinations = hero.GetValidMovementLocations().Select(x=>x.ToString()).ToList();
                hero.State = HeroState.Moving;
                var destination = await hero.SelectLocation(validDestinations);
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