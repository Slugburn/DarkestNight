using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public class SkipFreeAction : ICommand
    {
        public string Name => "Skip Free Action";
        public string Text => "Bypass your chance for a free action.";
        public bool RequiresAction => false;

        public Task ExecuteAsync(Hero hero)
        {
            // this will clear the free action
            hero.IsActionAvailable = false;
            return Task.CompletedTask;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.HasFreeAction;
        }
    }
}
