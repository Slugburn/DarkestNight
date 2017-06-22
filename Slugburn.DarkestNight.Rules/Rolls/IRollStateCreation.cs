namespace Slugburn.DarkestNight.Rules.Rolls
{
    public interface IRollStateCreation
    {
        IRollStateCreation Type(RollType type);
        IRollStateCreation Base(string name, int diceCount);
        IRollStateCreation Target(int targetNumber);
    }
}