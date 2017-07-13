using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Items
{
    class TreasureChest : Item, ICommand, ICallbackHandler<string>
    {
        public TreasureChest() : base("Treasure Chest")
        {
            Text = "Discard at any time to draw a new power card.";
        }

        public void Execute(Hero hero)
        {
            Owner.RemoveFromInventory(this);
            hero.DrawPower();
        }

        public bool IsAvailable(Hero hero)
        {
            return true;
        }

        public void HandleCallback(Hero hero, string data)
        {
            hero.ContinueTurn();
        }

    }
}
