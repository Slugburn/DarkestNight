﻿using System;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Actions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public interface IGiven: ITestRoot
    {
        IGameContext Game { get; }

        IGiven Location(string village, Action<LocationContext> def);
        IGiven ActingHero(Action<HeroContext> def = null);
    }
}