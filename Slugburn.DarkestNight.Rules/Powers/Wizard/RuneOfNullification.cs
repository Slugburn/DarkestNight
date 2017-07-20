namespace Slugburn.DarkestNight.Rules.Powers.Wizard
{
    internal class RuneOfNullification : ActionPower
    {
        public RuneOfNullification()
        {
            Name = "Rune of Nullification";
            StartingPower = true;
            Text = "Deactivate all Runes. Activate and choose a type of blight.";
            ActiveText = "That type of blight has no effect.";
        }
    }
}