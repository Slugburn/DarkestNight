using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public abstract class PowerAction : IAction
    {
        protected PowerAction(string name)
        {
            Name = name;
        }

        protected PowerAction()
        {
        }

        public string Name { get; set; }
        public abstract void Act(Hero hero);

        public virtual bool IsAvailable(Hero hero)
        {
            return hero.GetPower(Name).IsUsable(hero);
        }
    }
}
