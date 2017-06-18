using System;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class UnexpectedStateException : Exception
    {
        public UnexpectedStateException(HeroState actual, HeroState expected)
            :base($"Unexpected game state. Expected {expected} but was {actual}.")
        {
        }
    }
}