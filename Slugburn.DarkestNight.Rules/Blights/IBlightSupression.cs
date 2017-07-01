using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public interface IBlightSupression
    {
        string Name { get; }
        bool IsSupressed(IBlight blight, Hero hero = null);
    }
}
