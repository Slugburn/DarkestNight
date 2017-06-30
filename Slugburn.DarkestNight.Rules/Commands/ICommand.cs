using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Text { get; }
        void Execute(Hero hero);
        bool IsAvailable(Hero hero);
    }
}