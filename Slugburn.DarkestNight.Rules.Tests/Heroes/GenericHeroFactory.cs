using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests.Heroes
{
    public class GenericHeroFactory : HeroFactory
    {
        public new static Hero Create()
        {
            return ((HeroFactory) new GenericHeroFactory()).Create();
        }
        private GenericHeroFactory() : base("Joe", 5, 5)
        {
        }
    }
}
