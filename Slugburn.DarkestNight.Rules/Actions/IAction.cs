using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public interface IAction 
    {
        string Name { get; }
        string Text { get; }
        void Act(Hero hero);
        bool IsAvailable(Hero hero);
    }
}
