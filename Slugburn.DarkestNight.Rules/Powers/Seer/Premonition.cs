namespace Slugburn.DarkestNight.Rules.Powers.Seer
{
    internal class Premonition : TacticPower
    {
        public Premonition()
        {
            Name = "Premonition";
            StartingPower = true;
            Text = "Elude with 3 dice. Exhaust if you roll fewer than 2 successes.";
        }
    }
}