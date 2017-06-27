using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
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

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new CelerityAction());
        }

        private class CelerityAction : PowerAction, ICallbackHandler
        {
            public CelerityAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                DruidFormPower.DeactivateAllForms(hero);
                hero.State = HeroState.SelectingLocation;
                var validDestionations = hero.GetSpace().AdjacentLocations.Select(x=>x.ToString()).ToList();
                hero.Player.DisplayLocationSelection(validDestionations, Callback.ForAction(hero, this));
            }

            public void HandleCallback(Hero hero, string path, object data)
            {
                // Move to selected location
                var destination = (Location)data;
                hero.MoveTo(destination);

                // Allow player to pick a new form
                hero.State = HeroState.ChoosingAction;
                var formActions = hero.GetPowers<IDruidForm>().Where(x => x.IsUsable(hero)).Select(x => x.Name);
                var continueAction = new CelerityContinueAction();
                var availableActions = formActions.Concat(new[] { continueAction.Name }).ToList();
                hero.AddAction(continueAction);
                hero.AvailableActions = availableActions;
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