using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class NecromancerVm : INotifyPropertyChanged
    {
        private Callback<object> _callback;
        private Visibility _visibility;
        private string _text;
        private ICommand _command;

        public NecromancerVm()
        {
            Visibility = Visibility.Hidden;
        }

        public void Update(NecromancerModel model)
        {
            Visibility = Visibility.Visible;
            Text = model.Description;
            _callback = model.Callback;
            Command = new CommandHandler(Continue);
        }

        private void Continue()
        {
            Visibility = Visibility.Hidden;
            _callback.Handle(null);
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

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
