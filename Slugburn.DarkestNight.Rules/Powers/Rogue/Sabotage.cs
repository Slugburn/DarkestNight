using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Sabotage : ActionPower
    {
        public Sabotage()
        {
            Name = "Sabotage";
            Text = "Spend 1 Secrecy in the Necromancer's location to cause -1 Darkness.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) 
                && hero.CanSpendSecrecy
                && hero.Location == hero.Game.Necromancer.Location;
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new SabotageCommand(this));
        }

        private class SabotageCommand : PowerAction
        {
            public SabotageCommand(Sabotage power) : base(power) { }

            public override void Execute(Hero hero)
            {
                hero.SpendSecrecy(1);
                hero.Game.Darkness--;
                hero.Game.UpdatePlayerBoard();
            }
        }
    }
}