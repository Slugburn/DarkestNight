using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FalseLife : Bonus, IBonusAction
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
            hero.Add(new PreventMovementEffect(location => Exhausted && location == Location.Monastery));
        }

        public void Use(Hero hero)
        {
            if (!IsUsable(hero))
                throw new PowerNotUsableException(this);
            hero.GainGrace(1, hero.DefaultGrace);
            Exhaust(hero);
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Grace < hero.DefaultGrace && hero.Location != Location.Monastery;
        }
    }
}