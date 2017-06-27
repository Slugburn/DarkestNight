using System;

namespace Slugburn.DarkestNight.Rules.Powers
{
    internal class PowerNotUsableException : Exception
    {
        public PowerNotUsableException(string powerName) : base($"Power '{powerName}' is not usable.")
        {
        }
    }
}