using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfValor : Oath
    {
        public OathOfValor()
        {
            Name = "Oath of Valor";
            ActiveText = "+1 die in fights.";
            FulfillText = "Win a fight; You may activate any Oath immediately.";
            BreakText = "Attempt to elude; you lose 1 Grace.";
        }

        public override void Fulfill(Hero hero)
        {
            throw new System.NotImplementedException();
        }

        public override void Break(Hero hero)
        {
            throw new System.NotImplementedException();
        }
    }
}