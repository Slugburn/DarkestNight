using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Hide : StandardAction
    {
        public Hide() : base("Hide")
        {
            Text = "Refresh your powers and gain 1 Secrecy (up to 5).";
        }

        public override void Execute(Hero hero)
        {
            hero.RefreshPowers();
            if (hero.Secrecy < 5)
                hero.GainSecrecy(1, 5);
            hero.Triggers.Send(HeroTrigger.Hidden);
        }
    }
}
