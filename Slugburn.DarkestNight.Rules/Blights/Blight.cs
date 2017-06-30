using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class Blight : IBlight
    {
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
        public Location Location { get; set; }

        public void Win(Hero hero)
        {
            hero.Game.DestroyBlight(Id);
            hero.Triggers.Send(HeroTrigger.DestroyedBlight);
        }

        public abstract void Failure(Hero hero);

    }
}
