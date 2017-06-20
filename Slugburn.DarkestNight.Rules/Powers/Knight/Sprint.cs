using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class Sprint : TacticPower
    {
        public Sprint() : base(TacticType.Elude)
        {
            Name = "Sprint";
            StartingPower = true;
            Text = "Elude with 2 dice.";
        }
        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new PowerTactic { PowerName = Name, Type = TacticType.Elude, DiceCount = 2 });
        }
    }
}