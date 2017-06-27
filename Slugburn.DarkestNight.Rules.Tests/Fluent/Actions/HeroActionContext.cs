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


        public IHeroActionContext DrawsEvent(string eventName = null)
        {
            if (eventName != null)
            {
                // move event to first
                _hero.Game.Events.Remove(eventName);
                _hero.Game.Events.Insert(0, eventName);
            }
            if (_hero.Location == Location.Monastery)
                _hero.Location = Location.Village;
            _hero.DrawEvent();
            return this;
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

        public IHeroActionContext FacesEnemy(string enemyName)
        {
            _hero.FaceEnemy(enemyName);
            return this;
        }
    }
}