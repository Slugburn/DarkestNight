using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    public interface IItem : ISource
    {
        string Text { get; }
        Hero Owner { get; set; }
    }
}
