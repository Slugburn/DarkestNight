namespace Slugburn.DarkestNight.Rules.Powers.Wizard
{
    internal class Teleport : ActionPower
    {
        public Teleport()
        {
            Name = "Teleport";
            StartingPower = true;
            Text = "Exhaust to move directly to any location, gaining 2 Secrecy (up to 5).";
        }
    }
}