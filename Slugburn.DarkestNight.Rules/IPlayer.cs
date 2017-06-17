using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules
{
    public interface IPlayer
    {
        bool AskUsePower(string name, string description);
        int RollOne();
        TacticPower ChooseTactic(IEnumerable<TacticPower> choices);
        IEnumerable<int> RollDice(int count);
        List<Blight> ChooseBlights(List<Blight> choices, int count);
        int AssignRollToBlight(Blight blight, List<int> rolls);
        Location ChooseLocation(IEnumerable<Location> choices);
        List<Blight> ChooseBlights(List<Blight> choices, int min, int max);
        bool AskToRollAnotherDie(List<int> stateRoll);
    }
}
