﻿using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Players.Models;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface IPlayer
    {
        PlayerState State { get; set; }
        void DisplayEvent(PlayerEvent playerEvent);
        void DisplayConflict(PlayerConflict conflict);
        void DisplayPowers(ICollection<PlayerPower> powers);
        void DisplayBlightSelection(PlayerBlightSelection blightSelection, Callback callback);
        void DisplayLocationSelection(ICollection<string> locations, Callback callback);
        void DisplayNecromancer(PlayerNecromancer necromancer);
        void DisplayHeroSelection(PlayerHeroSelection view, Callback callback);
    }
}
