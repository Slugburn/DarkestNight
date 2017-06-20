using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public abstract class HeroTriggerHandler : ITriggerHandler<Hero>
    {
        public string Name { get; set; }
        public abstract void HandleTrigger(Hero hero, TriggerContext context);
    }
}
