using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class PrayerVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private ICommand _command;
        private int _graceGained;
        private string _roll;
        private Visibility _visibility;

        public PrayerVm(Game game)
        {
            _game = game;
            Visibility = Visibility.Hidden;
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility) return;
                _visibility = value;
                OnPropertyChanged();
            }
        }

        public string Roll
        {
            get { return _roll; }
            set
            {
                if (value == _roll) return;
                _roll = value;
                OnPropertyChanged();
            }
        }


        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (Equals(value, _command)) return;
                _command = value;
                OnPropertyChanged();
            }
        }

        public int GraceGained
        {
            get { return _graceGained; }
            set
            {
                if (value == _graceGained) return;
                _graceGained = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(PrayerModel model)
        {
            if (model == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            Visibility = Visibility.Visible;
            Roll = model.Roll.Select(x => x.ToString()).ToCsv();
            GraceGained = model.After - model.Before;
            Command = new CommandHandler(() =>
            {
                Visibility = Visibility.Hidden;
                _game.ActingHero.AcceptRoll();
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}