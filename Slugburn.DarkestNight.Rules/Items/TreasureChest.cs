﻿using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Items
{
    class TreasureChest : Item, ICommand, ICallbackHandler
    {
        public TreasureChest() : base("Treasure Chest")
        {
            Text = "Discard at any time to draw a new power card.";
        }

        public void Execute(Hero hero)
        {
            Owner.RemoveFromInventory(this);
            hero.DrawPower(Callback.For(hero, this));
        }

        public bool IsAvailable(Hero hero)
        {
            return true;
        }

        public void HandleCallback(Hero hero, object data)
        {
            // ?
        }

    }
}
