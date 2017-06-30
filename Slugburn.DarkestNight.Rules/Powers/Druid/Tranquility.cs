using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Tranquility : BonusPower
    {
        public Tranquility()
        {
            Name = "Tranquility";
            Text = "+3 to default Grace.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.DefaultGrace += 3;
        }
    }
}