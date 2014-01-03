using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IBlight
    {
        string Name { get;  }
        int Might { get; }

        void Defend(IHero hero);
    }
}
