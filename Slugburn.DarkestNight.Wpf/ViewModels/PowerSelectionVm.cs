using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class PowerSelectionVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private Visibility _visibility;
        private List<PowerCardVm> _cards;
        private PowerCardVm _selectedCard;
        private ICommand _command;

        public PowerSelectionVm(Game game)
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

        public List<PowerCardVm> Cards
        {
            get { return _cards; }
            set
            {
                if (Equals(value, _cards)) return;
                _cards = value;
                OnPropertyChanged();
            }
        }

        public PowerCardVm SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                if (Equals(value, _selectedCard)) return;
                _selectedCard = value;
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

        public void Update(IEnumerable<PowerModel> models, TaskCompletionSource<string> source)
        {
            Visibility = Visibility.Visible;
            Cards = models.Select(PowerCardVm.Create).ToList();
            SelectedCard = Cards.First();
            Command = new CommandHandler(() =>
            {
                Visibility = Visibility.Hidden;
                source.SetResult(SelectedCard.Name);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
