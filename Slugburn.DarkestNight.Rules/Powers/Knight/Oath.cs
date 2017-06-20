using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    abstract class Oath : ActivateablePower, IOath
    {
        protected Oath()
        {
            Text = "If no Oaths are active, activate until you fulfill or break.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new ActivateOathAction { Name = Name });
        }

        private class ActivateOathAction : PowerAction
        {
            public override void Act(Hero hero)
            {
                var power = (Oath)hero.GetPower(Name);
                power.Activate(hero);
            }
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.Triggers.RemoveBySource(Name);
            hero.Game.Triggers.RemoveBySource(Name);
            return true;
        }

        public string FulfillText { get; set; }
        public string BreakText { get; set; }
        public abstract void Fulfill(Hero hero);
        public abstract void Break(Hero hero);

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero)
                // Can not activate if any other oath powers are active
                   && !hero.Powers.Where(x => x is IOath).Cast<IOath>().Any(x => x.IsActive);
        }
    }
}