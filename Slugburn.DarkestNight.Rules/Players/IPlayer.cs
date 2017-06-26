using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface IPlayer
    {
        bool AskUsePower(string name, string description);
        PlayerState State { get; set; }
        void DisplayEvent(PlayerEvent playerEvent);
        void DisplayConflict(PlayerConflict conflict);
        void DisplayPowers(ICollection<PlayerPower> powers);
        void DisplayBlightSelection(PlayerBlightSelection blightSelection);
        void DisplayLocationSelection(ICollection<string> locations);
        void DisplayNecromancer(PlayerNecromancer necromancer);
    }
}
