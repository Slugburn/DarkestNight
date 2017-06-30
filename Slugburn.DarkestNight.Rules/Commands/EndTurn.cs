using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class EndTurn : ICommand
    {
        public string Name => "End Turn";

        public string Text => "Resolve the effects of any blights in your location that trigger at the end of a turn.\n"
                              + "If you spent your entire turn in the Monastery, gain 1 Secrecy (up to default).";

        public void Execute(Hero hero)
        {
            hero.TryToEndTurn();
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.CurrentEvent == null && hero.ConflictState == null;
        }

    }
}
