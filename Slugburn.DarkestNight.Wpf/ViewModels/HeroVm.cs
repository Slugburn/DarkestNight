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
        private List<HeroCommand> _commands;
        private SolidColorBrush _highlight;
        private ICommand _selectCommand;
        private HeroStatus _status;
        private List<HeroPowerVm> _powers;
        private List<ItemVm> _items;

        public HeroVm(Game game, HeroModel model)
        {
            _game = game;
            Name = model.Name;
            Status = new HeroStatus(model.Status);
            Powers = HeroPowerVm.Create(model.Powers);
            if (model.Commands != null)
                Commands = model.Commands.Select(c => new HeroCommand(_game, model.Name, c)).ToList();
        }

        public string Name { get; set; }

        public List<HeroCommand> Commands
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

        public List<HeroPowerVm> Powers
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class HeroCommand
    {
        private readonly Game _game;
        private readonly string _heroName;

        public HeroCommand(Game game, string heroName, CommandModel model)
        {
            _game = game;
            _heroName = heroName;
            Name = model.Name;
            Text = model.Text;
            Command = new CommandHandler(Execute);
        }

        public string Name { get; set; }
        public string Text { get; set; }

        public ICommand Command { get; set; }

        private void Execute()
        {
            _game.GetHero(_heroName).ExecuteCommand(Name);
        }

        public static List<HeroCommand> Create(Game game, string heroName, IEnumerable<CommandModel> commands)
        {
            return commands.Select(c=>new HeroCommand(game, heroName, c)).ToList();
        }
    }

}