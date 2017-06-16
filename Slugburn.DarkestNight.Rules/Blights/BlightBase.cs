using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class BlightBase : IBlight
    {
        public Blight Type { get; }

        protected BlightBase(Blight type)
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
