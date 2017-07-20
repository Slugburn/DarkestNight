namespace Slugburn.DarkestNight.Rules.Powers.Wizard
{
    internal class Invisibility : TacticPower
    {
        public Invisibility()
        {
            Name = "Invisibility";
            StartingPower = true;
            Text = "Activate.";
            ActiveText = "+2 dice when eluding. Deactivate and exhaust if you fail a combat or use another Tactic.";
        }
    }
}