using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerCommand
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static PlayerCommand FromCommand(ICommand command)
        {
            return new PlayerCommand {Name = command.Name, Text = command.Text};
        }

        public static IEnumerable<PlayerCommand> FromCommands(IEnumerable<ICommand> commands)
        {
            return commands.Select(FromCommand);
        }
    }
}
