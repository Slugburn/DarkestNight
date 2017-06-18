using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public interface IAction 
    {
        void Act(Hero hero);
        string Name { get; }
    }
}
