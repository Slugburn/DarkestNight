using Slugburn.DarkestNight.Rules.Commands;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public interface IActionFilter
    {
        string Name { get; }
        bool IsAllowed(ICommand command);
    }
}