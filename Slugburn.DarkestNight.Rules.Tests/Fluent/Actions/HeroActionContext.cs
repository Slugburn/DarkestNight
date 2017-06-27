using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class HeroActionContext : WhenContext, IHeroActionContext
    {
        private readonly Hero _hero;

        public HeroActionContext(Game game, FakePlayer player) : base(game, player)
        {
            _hero = game.ActingHero;
        }

        public IHeroActionContext RefreshesPower(string powerName)
        {
            var power = _hero.GetPower(powerName);
            power.Refresh();
            return this;
        }

        public IHeroActionContext MovesTo(string location)
        {
            _hero.MoveTo(location.ToEnum<Location>());
            return this;
        }
    }
}