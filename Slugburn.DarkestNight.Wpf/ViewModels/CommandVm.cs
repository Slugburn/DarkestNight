using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class CommandVm
    {
        private readonly Game _game;
        private readonly string _heroName;

        public CommandVm(Game game, string heroName, CommandModel model)
        {
            _game = game;
            _heroName = heroName;
            Name = model.Name;
            Text = model.Text;
            Command = new CommandHandler(Execute);
        }

        public CommandVm() { }

        public string Name { get; set; }
        public string Text { get; set; }

        public ICommand Command { get; set; }

        private void Execute()
        {
            _game.GetHero(_heroName).ExecuteCommand(Name);
        }

        public static List<CommandVm> Create(Game game, string heroName, IEnumerable<CommandModel> commands)
        {
            return commands.Select(c => new CommandVm(game, heroName, c)).ToList();
        }
    }
}