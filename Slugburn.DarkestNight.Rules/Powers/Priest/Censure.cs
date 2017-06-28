using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Censure : TacticPower
    {
        public Censure()
            : base(TacticType.Fight, 2)
        {
            Name = "Censure";
            Text = "Fight with 2d.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new PowerTactic { PowerName = Name, Type = TacticType.Fight, DiceCount = 2 });
        }
    }
}