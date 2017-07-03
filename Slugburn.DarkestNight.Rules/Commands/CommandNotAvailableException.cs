using System;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Commands
{
    internal class CommandNotAvailableException : Exception
    {
        public CommandNotAvailableException(Hero hero, ICommand command) : this(hero, command.Name)
        {
        }

        public CommandNotAvailableException(Hero hero, string commandName)
            : base($"Command '{commandName}' is not available to hero '{hero.Name}'.")
        {
        }
    }
}