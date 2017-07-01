using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class Blight : IBlight
    {
        private Space _space;
        public BlightType Type { get; }

        protected Blight(BlightType type)
        {
            Type = type;
        }

        public int Id { get; set; }
        public string Name { get; protected set; }
        public int Might { get; protected set; }
        public string EffectText { get; protected set; }
        public string DefenseText { get; protected set; }
        public Location Location => _space.Location;

        public void Win(Hero hero)
        {
            hero.Game.DestroyBlight(hero, Id);
            hero.Triggers.Send(HeroTrigger.DestroyedBlight);
        }

        public abstract void Failure(Hero hero);

        public void SetSpace(Space space)
        {
            _space = space;
        }
    }
}
