using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    internal class ActionNotAvailableException : Exception
    {
        public ActionNotAvailableException(Hero hero, IAction action) : base($"Action '{action.Name}' is not available to hero '{hero.Name}'.")
        {
        }
    }
}