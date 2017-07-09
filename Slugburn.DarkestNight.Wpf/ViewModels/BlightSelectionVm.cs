using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class BlightSelectionVm : INotifyPropertyChanged
    {
        private Callback _callback;
        private ICommand _command;
        private SelectionMode _selectionMode;
        private int _max;
        private List<BlightVm> _blights;
        private string _title;
        private Visibility _visibility;

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

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public List<BlightVm> Blights
        {
            get { return _blights; }
            set
            {
                if (Equals(value, _blights)) return;
                _blights = value;
                OnPropertyChanged();
            }
        }

        public SelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set
            {
                if (value == _selectionMode) return;
                _selectionMode = value;
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

        public void Update(BlightSelectionModel model)
        {
            Visibility = Visibility.Visible;
            Title = model.Title;
            _max = model.Max;
            Blights = BlightVm.CreateBlights(model.Blights);
            Blights.First().IsSelected = true;
            SelectionMode = _max == 1 ? SelectionMode.Single : SelectionMode.Multiple;
            _callback = model.Callback;
            var command = new CommandHandler(ExecuteCommand, IsCommandEnabled);
            Command = command;

            foreach (var blight in Blights)
                blight.PropertyChanged += (sender, e) => command.OnCanExecuteChanged();
        }

        private void ExecuteCommand()
        {
            Visibility = Visibility.Hidden;
            var selected = Blights.Where(x => x.IsSelected).Select(x => x.Id);
            _callback.Handle(selected);
        }

        private bool IsCommandEnabled()
        {
            var count = Blights.Count(x => x.IsSelected);
            return count >= 1 && count <= _max;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
