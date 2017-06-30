using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    internal class CommandNotAvailableException : Exception
    {
        public CommandNotAvailableException(Hero hero, ICommand command)
            : base($"Command '{command.Name}' is not available to hero '{hero.Name}'.")
        {
        }
    }
}