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
            if (!base.IsUsable(hero)) return false;
            var hasActiveOath = hero.Powers.WhereIs<IOath>().Any(x => x.IsActive);
            return !hasActiveOath;
        }
    }
}