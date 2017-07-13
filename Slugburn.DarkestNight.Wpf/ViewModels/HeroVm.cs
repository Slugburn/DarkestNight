using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class HeroVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private List<CommandVm> _commands;
        private SolidColorBrush _highlight;
        private ICommand _selectCommand;
        private HeroStatus _status;
        private List<PowerVm> _powers;
        private List<ItemVm> _items;

        public HeroVm(Game game, HeroModel model)
        {
            _game = game;
            Name = model.Name;
            Status = new HeroStatus(model.Status);
            Powers = PowerVm.Create(model.Powers);
            if (model.Commands != null)
                Commands = model.Commands.Select(c => new CommandVm(_game, model.Name, c)).ToList();
        }

        public HeroVm()
        {
            Powers = new List<PowerVm>();
            Commands = new List<CommandVm>();
            Items = new List<ItemVm>();
            Highlight = new SolidColorBrush(Colors.White);
        }

        public string Name { get; set; }

        public List<CommandVm> Commands
        {
            get { return _commands; }
            set
            {
                if (Equals(value, _commands)) return;
                _commands = value;
                OnPropertyChanged();
            }
        }

        public HeroStatus Status
        {
            get { return _status; }
            set
            {
                if (Equals(value, _status)) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        public List<PowerVm> Powers
        {
            get { return _powers; }
            set
            {
                if (Equals(value, _powers)) return;
                _powers = value;
                OnPropertyChanged();
            }
        }

        public List<ItemVm> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Highlight
        {
            get { return _highlight; }
            set
            {
                if (Equals(value, _highlight)) return;
                _highlight = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectCommand
        {
            get { return _selectCommand; }
            set
            {
                if (Equals(value, _selectCommand)) return;
                _selectCommand = value;
                OnPropertyChanged();
            }
        }

        public void ReceiveItem(string fromName, int itemId)
        {
            var fromHero = _game.GetHero(fromName);
            var toHero = _game.GetHero(Name);
            fromHero.TradeItemTo(toHero, itemId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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

        public CommandVm()
        {
        }

        public string Name { get; set; }
        public string Text { get; set; }

        public ICommand Command { get; set; }

        private void Execute()
        {
            _game.GetHero(_heroName).ExecuteCommand(Name);
        }

        public static List<CommandVm> Create(Game game, string heroName, IEnumerable<CommandModel> commands)
        {
            return commands.Select(c=>new CommandVm(game, heroName, c)).ToList();
        }
    }

}