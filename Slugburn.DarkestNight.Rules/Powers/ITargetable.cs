namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface ITargetable
    {
        void SetTarget(string targetName);
        string GetTarget();
    }
}
