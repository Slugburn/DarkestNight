namespace Slugburn.DarkestNight.Rules.Powers.Wizard
{
    internal class RuneOfInterference : ActionPower
    {
        public RuneOfInterference()
        {
            Name = "Rune of Interference";
            Text = "Deactivate all Runes. Activate.";
            ActiveText = "Roll 1 die when a blight is created. If you roll a 6, destroy it.";
        }
    }
}