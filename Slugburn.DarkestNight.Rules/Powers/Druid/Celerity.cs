using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
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

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new CelerityAction());
        }

        private class CelerityAction : PowerAction
        {
            public CelerityAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                DruidFormPower.DeactivateAllForms(hero);
                hero.State = HeroState.SelectingLocation;
                hero.SetLocationSelectedHandler(new CelerityLocationSelectedHandler());
            }

            private class CelerityLocationSelectedHandler : ILocationSelectedHandler
            {
                public void Handle(Hero hero, Location location)
                {
                    // Move to selected location
                    hero.MoveTo(location);

                    // Allow player to pick a new form
                    hero.State = HeroState.ChoosingAction;
                    var formActions = hero.GetPowers<IDruidForm>().Where(x => x.IsUsable(hero)).Select(x => x.Name);
                    var continueAction = new CelerityContinueAction();
                    var availableActions = formActions.Concat(new[] { continueAction.Name }).ToList();
                    hero.AddAction(continueAction);
                    hero.AvailableActions = availableActions;
                }

            }
            private class CelerityContinueAction : IAction
            {
                public string Name => "Continue";

                public void Act(Hero hero)
                {
                    hero.RemoveAction(Name);
                    hero.IsActionAvailable = false;
                }

                public bool IsAvailable(Hero hero)
                {
                    return true;
                }
            }
        }
    }
}