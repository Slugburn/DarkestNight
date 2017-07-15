using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    class Chapel : ActivateOnSpacePower
    {
        private readonly ChapelPray _action;

        public Chapel()
        {
            Name = "Chapel";
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes may pray there.";
            _action = new ChapelPray(this);
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.CanSpendSecrecy;
        }

        protected override void PayActivationCost()
        {
            Owner.SpendSecrecy(1);
        }

        protected override void ActivateEffect()
        {
            Target.AddAction(_action);
        }

        internal class ChapelPray : StandardAction
        {
            private readonly IActionPower _power;
            private readonly Pray _pray;

            public ChapelPray(IActionPower power) : base("Pray [Chapel]")
            {
                _power = power;
                _pray = new Pray();
                Text = _pray.Text;
            }

            public override bool IsAvailable(Hero hero)
            {
                return base.IsAvailable(hero)
                       && hero.Grace < hero.DefaultGrace 
                       && hero.CanGainGrace()
                       && !_power.IsExhausted;
            }

            public override void Execute(Hero hero)
            {
                _pray.Execute(hero);
            }
        }
    }
}