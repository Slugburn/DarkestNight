using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Hide : IAction
    {
        public string Name => "Hide";

        public string Text => "Refresh your powers and gain 1 Secrecy (up to 5).";

        public void Act(Hero hero)
        {
            hero.IsActionAvailable = false;
            hero.RefreshPowers();
            if (hero.Secrecy < 5)
                hero.GainSecrecy(1, 5);
            hero.Triggers.Send(HeroTrigger.Hidden);
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.IsActionAvailable && (hero.Secrecy < 5 || hero.Powers.Any(power=>power.Exhausted));
        }
    }
}
