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
    }
}