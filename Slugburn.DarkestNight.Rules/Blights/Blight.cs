using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class Blight : IBlight
    {
        public BlightType Type { get; }

        protected Blight(BlightType type)
        {
            Type = type;
        }

        public string Name { get; protected set; }
        public int Might { get; protected set; }
        public string EffectText { get; protected set; }
        public string DefenseText { get; protected set; }

        public abstract void Defend(Hero hero);

    }
}
