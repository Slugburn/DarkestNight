using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class CommandModel
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static CommandModel FromCommand(ICommand command)
        {
            return new CommandModel {Name = command.Name, Text = command.Text};
        }

        public static IEnumerable<CommandModel> FromCommands(IEnumerable<ICommand> commands)
        {
            return commands.Select(FromCommand);
        }
    }
}
