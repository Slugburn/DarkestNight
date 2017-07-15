using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Camouflage : TacticPower
    {
        public Camouflage()
        {
            Name = "Camouflage";
            StartingPower = true;
            Text = "Elude with 2 dice.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new PowerTactic(this, TacticType.Elude, 2));
        }
    }
}