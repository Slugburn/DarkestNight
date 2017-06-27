using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    internal abstract class DruidFormPower : ActivateablePower, IDruidForm
    {
        public override void Learn(Hero hero)
        {
//            base.Learn(hero);
            AddFormActions(hero, Name);
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.IsActionAvailable = false;
            hero.CanGainGrace = false;
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.CanGainGrace = true;
            return true;
        }

        private static void AddFormActions(Hero hero, string powerName)
        {
            if (!hero.HasAction(DeactivateForm.ActionName))
                hero.AddAction(new DeactivateForm());
            hero.AddAction(new ActivateForm(powerName));
        }

        internal static void DeactivateAllForms(Hero hero)
        {
            hero.Powers.Where(x => x is IDruidForm).Cast<IDruidForm>().ToList().ForEach(x => x.Deactivate(hero));
        }

        private class ActivateForm : PowerAction
        {
            public ActivateForm(string name) : base(name)
            {
            }

            public override void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                DeactivateAllForms(hero);
                var power = (IDruidForm) hero.GetPower(Name);
                power.Activate(hero);
            }
        }

        private class DeactivateForm : IAction
        {
            public const string ActionName = "Deactivate Form";
            public string Name => ActionName;

            public void Act(Hero hero)
            {
                DeactivateAllForms(hero);
                hero.IsActionAvailable = false;
            }

            public bool IsAvailable(Hero hero)
            {
                return hero.IsActionAvailable && hero.GetPowers<IDruidForm>().Any(x => x.IsActive);
            }
        }
    }
}