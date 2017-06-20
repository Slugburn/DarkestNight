namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfPurging : Oath
    {
        public OathOfPurging()
        {
            Name = "Oath of Purging";
            StartingPower = true;
            ActiveText = "+2 dice in fights when attacking blights.";
            FulfillText = "Destroy a blight; you gain 1 Grace.";
            BreakText = "Enter the Monastery; you lose 1 Grace.";
        }
    }
}