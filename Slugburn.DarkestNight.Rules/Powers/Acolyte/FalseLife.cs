using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FalseLife : BonusPower
    {
        private const string PowerName = "False Life";

        public FalseLife()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Exhaust at any time while not at the Monastery to gain 1 Grace (up to default). You may not enter the Monastery while this power is exhausted.";
        }

        protected override void OnLearn()
        {
            Owner.AddCommand(new FalseLifeAction(this));
            Owner.Add(new PreventMovementEffect(location => Exhausted && location == Location.Monastery));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Grace < hero.DefaultGrace && hero.Location != Location.Monastery && hero.CanGainGrace();
        }

        internal class FalseLifeAction : PowerCommand
        {
            public FalseLifeAction(IPower power) : base(power, false)
            {
            }

            public override void Execute(Hero hero)
            {
                var power = hero.GetPower(Name);
                if (!power.IsUsable(hero))
                    throw new CommandNotAvailableException(hero, this);
                hero.GainGrace(1, hero.DefaultGrace);
                power.Exhaust(hero);
            }
        }

    }
}