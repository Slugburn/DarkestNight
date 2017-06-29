using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    public interface IItem : ISource
    {
        Hero Owner { get; set; }
    }
}
