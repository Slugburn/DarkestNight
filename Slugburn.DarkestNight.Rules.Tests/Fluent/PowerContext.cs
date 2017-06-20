using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class PowerContext
    {
        private readonly Hero _hero;
        private readonly IPower _power;

        public PowerContext(Hero hero, IPower power)
        {
            _hero = hero;
            _power = power;
        }

        public PowerContext IsActive()
        {
            ((IActivateable)_power).Activate(_hero);
            _hero.IsActionAvailable = true;
            return this;
        }

        public PowerContext IsExhausted()
        {
            _power.Exhaust(_hero);
            return this;
        }
    }
}
