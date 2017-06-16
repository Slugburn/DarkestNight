using System;

namespace Slugburn.DarkestNight.Rules.Powers
{
    internal class PowerNotUsableException : Exception
    {
        public PowerNotUsableException(IPower power) : base($"Power '{power.Name}' is not usable.")
        {
        }
    }
}