namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class RecklessAbandon : TacticPower
    {
        public RecklessAbandon() : base(TacticType.Fight)
        {
            Name = "Reckless Abandon";
            Text = "Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.";
        }
    }
}