namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Intercession : Bonus
    {
        public Intercession()
        {
            Name = "Intercession";
            StartingPower = true;
            Text = "Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.";
        }
    }
}