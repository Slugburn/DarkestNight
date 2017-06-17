namespace Slugburn.DarkestNight.Rules.Blights
{
    public class BlightDieAssignment
    {
        public static BlightDieAssignment Create(Blight blight, int dieValue)
        {
            return new BlightDieAssignment { Blight = blight, DieValue = dieValue};
        }
        public Blight Blight { get; set; }
        public int DieValue { get; set; }
    }
}