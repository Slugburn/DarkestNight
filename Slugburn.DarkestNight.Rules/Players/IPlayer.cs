using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface IPlayer
    {
        bool AskUsePower(string name, string description);
        Location ChooseLocation(IEnumerable<Location> choices);
        List<Blight> ChooseBlights(ICollection<Blight> choices, int min, int max);
        PlayerState State { get; set; }
        void DisplayEvent(PlayerEvent playerEvent);
        void DisplayConflict(PlayerConflict conflict);
        void DisplayPowers(ICollection<PlayerPower> powers);
    }
}
