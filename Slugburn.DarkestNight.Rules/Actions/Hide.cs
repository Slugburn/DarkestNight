using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Hide : IAction
    {
        public string Name => "Hide";

        public void Act(Hero hero)
        {
            hero.IsActionAvailable = false;
            hero.Triggers.Send(HeroTrigger.Hiding);
            hero.RefreshPowers();
            if (hero.Secrecy < 5)
                hero.GainSecrecy(1, 5);
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.State == HeroState.ChoosingAction;
        }
    }
}
