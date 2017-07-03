using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class StandardAction : IAction
    {
        protected StandardAction(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }
        public string Text { get; set; }
        public abstract void Execute(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return hero.State == HeroState.TakingTurn && hero.IsTakingTurn && hero.IsActionAvailable && hero.CurrentEvent == null;
        }
    }
}
