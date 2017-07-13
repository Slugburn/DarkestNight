using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Text { get; }
        bool RequiresAction { get; }
        bool IsAvailable(Hero hero);
        Task ExecuteAsync(Hero hero);
    }
}