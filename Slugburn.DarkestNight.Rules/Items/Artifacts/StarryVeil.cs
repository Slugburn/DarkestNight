namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class StarryVeil : Artifact
    {
        public StarryVeil() : base("Starry Veil")
        {
            Text = "When any hero at your location draws an event with a Fate of 5 or more, "
                   + "they may discard it and draw another.";
        }
    }
}
