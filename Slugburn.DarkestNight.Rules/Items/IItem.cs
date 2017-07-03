using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    public interface IItem : ISource
    {
        int Id { get; }
        string Text { get; }
        Hero Owner { get; }
        void SetOwner(Hero hero);
    }
}
