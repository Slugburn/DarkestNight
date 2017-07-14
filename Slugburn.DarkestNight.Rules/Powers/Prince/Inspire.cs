namespace Slugburn.DarkestNight.Rules.Powers.Prince {
    class Inspire : ActionPower
    {
        public Inspire()
        {
            Name = "Inspire";
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Deactivate before any die roll for +3d.";
        }

    }
}