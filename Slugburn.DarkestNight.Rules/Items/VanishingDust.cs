using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Items
{
    class VanishingDust : Item, ICommand
    {
        public VanishingDust() : base("Vanishing Dust")
        {
            Text = "Discard after a failed elusion roll to make it a success.";
        }

        public bool IsAvailable(Hero hero)
        {
            var roll = hero.CurrentRoll;
            return roll != null && !roll.Win && roll.ModifierType == ModifierType.EludeDice;
        }

        public void Execute(Hero hero)
        {
            hero.RemoveFromInventory(this);
            hero.CurrentRoll.ForceWin();
            hero.ConflictState.Resolve(hero);
            hero.AcceptRoll();
        }

    }
}
