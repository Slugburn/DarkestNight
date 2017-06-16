namespace Slugburn.DarkestNight.Rules
{
    public class RollContext
    {
        public RollContext(int dieCount)
        {
            DieCount = dieCount;
        }

        public int DieCount { get; set; }
    }
}