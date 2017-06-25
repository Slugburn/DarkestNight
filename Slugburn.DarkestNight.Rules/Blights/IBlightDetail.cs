using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IBlightDetail : IConflict
    {
        Blight Type { get; }
        int Might { get; }
        string EffectText { get; }
        string DefenseText { get;  }

        void Failure(Hero hero);
    }
}
