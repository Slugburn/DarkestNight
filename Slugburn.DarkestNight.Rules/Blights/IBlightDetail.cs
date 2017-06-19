using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IBlightDetail
    {
        Blight Type { get; }
        string Name { get; }
        int Might { get; }
        string EffectText { get; }
        string DefenseText { get;  }

        void Defend(Hero hero);
    }
}
