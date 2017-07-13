using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class EndTurn : ICommand
    {
        public string Name => "End Turn";

        public string Text => "Resolve the effects of any blights in your location that trigger at the end of a turn.\n"
                              + "If you spent your entire turn in the Monastery, gain 1 Secrecy (up to default).";

        public bool RequiresAction => false;

        public Task ExecuteAsync(Hero hero)
        {
            hero.EndTurn();
            return Task.CompletedTask;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsTakingTurn && hero.State == HeroState.TakingTurn && hero.CurrentEvent == null && hero.ConflictState == null;
        }

    }
}
