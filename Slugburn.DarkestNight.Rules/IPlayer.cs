using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules
{
    public interface IPlayer
    {
        bool AskUsePower(string name, string description);
        int RollOne();
        IEnumerable<int> RollDice(int count);
        Location ChooseLocation(IEnumerable<Location> choices);
        List<Blight> ChooseBlights(List<Blight> choices, int min, int max);
    }
}
