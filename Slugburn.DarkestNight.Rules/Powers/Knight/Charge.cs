using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class Charge : TacticPower
    {
        private const string PowerName = "Charge";

        public Charge() : base()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Fight with 2 dice.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new PowerTactic { PowerName = PowerName, Type = TacticType.Fight, DiceCount = 2 });
        }
    }
}