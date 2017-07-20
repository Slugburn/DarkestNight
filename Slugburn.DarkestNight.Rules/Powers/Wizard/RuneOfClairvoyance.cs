namespace Slugburn.DarkestNight.Rules.Powers.Wizard
{
    internal class RuneOfClairvoyance : ActionPower
    {
        public RuneOfClairvoyance()
        {
            Name = "Rune of Clairvoyance";
            Text = "Deactivate all Runes. Activate.";
            ActiveText = "At the start of your turn, look at the top card of any deck; put it on the top or bottom of that deck.";
        }
    }
}