using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    internal abstract class DruidFormPower : ActivateablePower, IDruidForm
    {
        protected override void OnLearn()
        {
            AddFormActions();
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.IsGraceGainBlocked = true;
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.IsGraceGainBlocked = false;
            return true;
        }

        private void AddFormActions()
        {
            if (!Owner.HasCommand(DeactivateForm.ActionName))
                Owner.AddCommand(new DeactivateForm());
            Owner.AddCommand(new ActivateForm(this));
        }

        internal static void DeactivateAllForms(Hero hero)
        {
            hero.Powers.OfType<IDruidForm>().ToList().ForEach(x => x.Deactivate(hero));
        }

        private class ActivateForm : ActivatePowerAction
        {
            public ActivateForm(IActionPower power) : base(power)
            {
            }

            public override Task ExecuteAsync(Hero hero)
            {
                DeactivateAllForms(hero);
                return base.ExecuteAsync(hero);
            }
        }

        private class DeactivateForm : StandardAction
        {
            public const string ActionName = "Deactivate Form";

            public DeactivateForm() : base(ActionName)
            {
                Text = "Deactivate all Forms.";
            }

            public override Task ExecuteAsync(Hero hero)
            {
                DeactivateAllForms(hero);
                return Task.CompletedTask;
            }

            public override bool IsAvailable(Hero hero)
            {
                return base.IsAvailable(hero) &&  hero.Powers.OfType<IDruidForm>().Any(x => x.IsActive);
            }
        }
    }
}