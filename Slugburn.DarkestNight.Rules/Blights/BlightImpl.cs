using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class BlightImpl : IBlight
    {
        public string Name { get; protected set; }
        public int Might { get; protected set; }
        public abstract void Defend(IHero hero);

    }
}
