﻿using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Spaces
{
    public static class SpaceExtension
    {
        public static IEnumerable<Hero> GetHeroes(this Space space, Game game)
        {
            return game.Heroes.Where(hero => hero.Location == space.Location);
        }
    }
}
