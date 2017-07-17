using Slugburn.DarkestNight.Rules.Conflicts;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IBlight : IConflict
    {
        int Id { get; }
        Location Location { get; }
        BlightType Type { get; }
        int Might { get; }
        string EffectText { get; }
        string DefenseText { get;  }
        bool IsSupressed { get; }
    }
}
