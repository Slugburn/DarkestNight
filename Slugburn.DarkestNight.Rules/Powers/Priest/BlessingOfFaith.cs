namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfFaith : ActionPower
    {
        public BlessingOfFaith()
        {
            Name = "Blessing of Faith";
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Gain an extra Grace (up to default) when praying.";
        }
    }
}