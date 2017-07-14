using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    class Chapel : ActivateablePower
    {
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
            hero.Space.AddAction(action);
            hero.SpendSecrecy(1);
//            foreach (var h in hero.Space.GetHeroes(hero.Game))
//                h.UpdateAvailableCommands();
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
    }
}