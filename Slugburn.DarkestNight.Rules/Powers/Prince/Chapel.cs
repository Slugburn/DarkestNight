using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    class Chapel : ActivateablePower, ITargetable
    {
        private Space _target;

        public Chapel()
        {
            Name = "Chapel";
            Text = "Spend 1 Secrecy to activate in your location.";
            ActiveText = "Heroes may pray there.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.CanSpendSecrecy;
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            var action = new ChapelPray(this);
            if (_target == null)
            {
                hero.SpendSecrecy(1);
                _target = hero.Space;
            }
            _target.AddAction(action);
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
                       && !_power.Exhausted;
            }

            public override void Execute(Hero hero)
            {
                _pray.Execute(hero);
            }
        }

        public void SetTarget(string targetName)
        {
            var location = targetName.ToEnum<Location>();
            _target = Owner.Game.Board[location];
        }

        public string GetTarget()
        {
            return _target.Location.ToString();
        }
    }
}