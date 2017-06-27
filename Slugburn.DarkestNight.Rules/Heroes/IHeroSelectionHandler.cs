namespace Slugburn.DarkestNight.Rules.Heroes
{
    public interface IHeroSelectionHandler
    {
        void Handle(Hero hero, Hero selectedHero);
    }
}