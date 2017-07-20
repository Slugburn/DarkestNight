namespace Slugburn.DarkestNight.Rules.Powers.Seer
{
    internal class ProphecyOfFortune : BonusPower
    {
        public ProphecyOfFortune()
        {
            Name = "Prophecy of Fortune";
            StartingPower = true;
            Text = "Exhaust at the start of any hero's turn. That hero gains +1 die on all rolls this turn.";
        }
    }
}