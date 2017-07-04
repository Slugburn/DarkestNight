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
    public class Hero : INotifyPropertyChanged
    {
        private readonly Game _game;
        private List<HeroCommand> _commands;
        private HeroStatus _status;
        private SolidColorBrush _highlight;
        private ICommand _selectCommand;

        public Hero(Game game, HeroModel model)
        {
            _game = game;
            Name = model.Name;
            Status = new HeroStatus(model.Status);
            if (model.Commands != null)
                Commands = model.Commands.Select(c => new HeroCommand(_game, model.Name , c)).ToList();
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
}
