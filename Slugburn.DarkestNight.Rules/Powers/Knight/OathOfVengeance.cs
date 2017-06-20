namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class OathOfVengeance : Oath
    {
        public OathOfVengeance()
        {
            Name = "Oath of Vengeance";
            Text = "If no Oaths are active, activate until you fulfill or break.";
            ActiveText = "Add 1 to highest die when fighting the Necormancer.";
            FulfillText = "Win fight versus the Necromancer; you get a free action.";
            BreakText = "Hide or search; you lose 1 Grace.";
        }

    }
}