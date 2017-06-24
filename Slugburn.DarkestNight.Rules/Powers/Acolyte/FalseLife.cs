using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FalseLife : Bonus
    {
        public FalseLife()
        {
            Name = "False Life";
            StartingPower = true;
            Text = "Exhaust at any time while not at the Monastery to gain 1 Grace (up to default). You may not enter the Monastery while this power is exhausted.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new FalseLifeAction {Name = Name});
            hero.Add(new PreventMovementEffect(location => Exhausted && location == Location.Monastery));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Grace < hero.DefaultGrace && hero.Location != Location.Monastery;
        }

        internal class FalseLifeAction : PowerAction
        {
            public override void Act(Hero hero)
            {
                var power = hero.GetPower(Name);
                if (!power.IsUsable(hero))
                    throw new PowerNotUsableException(power);
                hero.GainGrace(1, hero.DefaultGrace);
                power.Exhaust(hero);
            }
        }

    }
}