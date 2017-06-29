using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class EndTurn : IAction
    {
        public string Name => "End Turn";

        public string Text => "Resolve the effects of any blights in your location that trigger at the end of a turn.\n"
                              + "If you spent your entire turn in the Monastery, gain 1 Secrecy (up to default).";

        public void Act(Hero hero)
        {
            if (hero.IsAffectedByBlight(Blight.Spies))
            {
                var space = hero.GetSpace();
                var spies = space.Blights.Where(x => x == Blight.Spies);
                foreach (var spy in spies)
                    hero.LoseSecrecy("Spies");
            }
            hero.IsTakingTurn = false;
            hero.IsTurnTaken = true;
            hero.Triggers.Send(HeroTrigger.EndOfTurn);
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn;
        }

    }
}
