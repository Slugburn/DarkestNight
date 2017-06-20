namespace Slugburn.DarkestNight.Rules.Blights
{
    public class TargetDieAssignment
    {
        public static TargetDieAssignment Create(int targetId, int dieValue)
        {
            return new TargetDieAssignment { TargetId = targetId, DieValue = dieValue};
        }
        public int TargetId { get; set; }
        public int DieValue { get; set; }
    }
}