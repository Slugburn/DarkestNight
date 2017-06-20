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
            hero.Triggers.UnregisterAll(Name);
            hero.Game.Triggers.UnregisterAll(Name);
            return true;
        }

        public string FulfillText { get; set; }
        public string BreakText { get; set; }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero)
                // Can not activate if any other oath powers are active
                   && !hero.Powers.Where(x => x is IOath).Cast<IOath>().Any(x => x.IsActive);
        }
    }
}