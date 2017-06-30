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
            hero.AddAction(new CelerityAction(this));
        }

        private class CelerityAction : PowerAction, ICallbackHandler
        {
            public CelerityAction(IActionPower power) : base(power)
            {
            }

            public override void Execute(Hero hero)
            {
                DruidFormPower.DeactivateAllForms(hero);
                var validDestionations = hero.GetSpace().AdjacentLocations.Select(x=>x.ToString()).ToList();
                hero.Player.DisplayLocationSelection(validDestionations, Callback.ForCommand(hero, this));
            }

            public void HandleCallback(Hero hero, string path, object data)
            {
                // Move to selected location
                var destination = (Location)data;
                hero.MoveTo(destination);

                // Allow player to pick a new form
                hero.AddFreeAction(p=>p is IDruidForm);
            }

            private class CelerityContinueAction : IAction
            {
                public string Name => "Continue";

                public void Execute(Hero hero)
                {
                    hero.RemoveAction(Name);
                }

                public bool IsAvailable(Hero hero)
                {
                    return true;
                }

                public string Text => "Continue";
            }
        }
    }
}