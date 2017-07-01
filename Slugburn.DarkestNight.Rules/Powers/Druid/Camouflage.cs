using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Camouflage : TacticPower
    {
        public Camouflage() : base()
        {
            Name = "Camouflage";
            StartingPower = true;
            Text = "Elude with 2 dice.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new CamouflageTactic());
        }

        private class CamouflageTactic : PowerTactic
        {
            public CamouflageTactic()
            {
                PowerName = "Camouflage";
                Type = TacticType.Elude;
                DiceCount = 2;
            }
        }
    }
}