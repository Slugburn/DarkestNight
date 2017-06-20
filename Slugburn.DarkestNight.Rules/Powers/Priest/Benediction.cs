namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Benediction : ActionPower
    {
        public Benediction()
        {
            Name = "Benediction";
            StartingPower = true;
            Text = "One hero at your location gains 1 Grace (up to default). If they now have more Grace than you, you gain 1 Grace.";
        }
    }
}