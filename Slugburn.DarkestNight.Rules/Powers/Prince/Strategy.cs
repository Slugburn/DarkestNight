using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Strategy : TacticPower
    {
        public Strategy()
        {
            Name = "Strategy";
            StartingPower = true;
            Text = "Fight with 2d.";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddTactic(new PowerTactic(this, TacticType.Fight, 2));
        }
    }
}