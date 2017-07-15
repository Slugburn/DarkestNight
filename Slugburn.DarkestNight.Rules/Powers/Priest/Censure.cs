using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Censure : TacticPower
    {
        public Censure()
            : base()
        {
            Name = "Censure";
            Text = "Fight with 2d.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new PowerTactic(this, TacticType.Fight, 2));
        }
    }
}