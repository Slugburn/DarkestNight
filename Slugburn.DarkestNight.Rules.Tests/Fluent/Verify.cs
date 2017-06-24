﻿using Slugburn.DarkestNight.Rules.Tests.Fluent.Assertions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public static class Verify
    {
        public static HeroVerification Hero => new HeroVerification();
        public static GameVerification Game => new GameVerification();

        public static PowerVerification Power(string name) => new PowerVerification(name);

        public static LocationVerification Location(string location) => new LocationVerification(location);
    }
}
