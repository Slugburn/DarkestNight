using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    public class GenericHeroFactory : HeroFactory
    {
        private GenericHeroFactory() : base("Joe", 5, 5)
        {
        }

        public new static Hero Create()
        {
            return ((HeroFactory) new GenericHeroFactory()).Create();
        }
    }
}