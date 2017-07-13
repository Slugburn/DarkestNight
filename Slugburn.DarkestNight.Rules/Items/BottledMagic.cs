using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Items
{
    public class BottledMagic :Item, ICommand
    {
        public BottledMagic() : base("Bottled Magic")
        {
            Text = "Discard after a fight roll to add 3d to that roll.";
        }

        public Task ExecuteAsync(Hero hero)
        {
            Owner.RemoveFromInventory(this);
            var roll = Die.Roll(3);
            hero.CurrentRoll.ActualRoll.AddRange(roll);
            hero.CurrentRoll.AdjustRoll();
            return Task.CompletedTask;
        }

        public bool IsAvailable(Hero hero)
        {
            var state = hero.ConflictState;
            // There must be an available roll to add to
            if (state?.Roll == null) return false;
            // It must be a fight roll
            return state.SelectedTactic.Type == TacticType.Fight;
        }

    }
}
