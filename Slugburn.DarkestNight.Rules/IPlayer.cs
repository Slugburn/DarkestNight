using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    public interface IPlayer
    {
        bool AskUsePower(string name, string description);
        int RollOne();
        Tactic ChooseTactic(IEnumerable<Tactic> choices);
        int ChooseDieCount(params int[] choices);
        IEnumerable<int> RollDice(int count);
        List<BlightType> ChooseBlights(List<BlightType> choices, int count);
        int AssignRollToBlight(BlightType blight, List<int> rolls);
    }
}
