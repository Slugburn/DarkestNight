using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Eavesdrop : ActionPower
    {
        public Eavesdrop()
        {
            Name = "Eavesdrop";
            StartingPower = true;
            Text = "Spend 1 Secrecy to search with 2 dice.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) 
                && hero.CanSpendSecrecy
                && hero.Location != Location.Monastery;
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new EavesdropCommand(this));
        }

        internal class EavesdropCommand : PowerAction
        {
            public EavesdropCommand(IActionPower power) : base(power) { }

            public override void Execute(Hero hero)
            {
                hero.SpendSecrecy(1);
                Search.DoSearch(hero, 2);
            }
        }
    }
}