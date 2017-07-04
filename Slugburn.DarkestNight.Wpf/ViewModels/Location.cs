using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Location : INotifyPropertyChanged
    {
        private string _searchTarget;
        private List<string> _tokens;
        private List<Blight> _blights;
        private Brush _highlight = new SolidColorBrush(Colors.LightGray);
        private ICommand _selectCommand;

        public Location(LocationModel model)
        {
            Name = model.Name;
            SearchTarget = model.SearchTarget > 0 ? $"{model.SearchTarget}+" : null;
            Tokens = model.Tokens;
            Blights = model.Blights.Select(b => new Blight(b)).ToList();
        }

        public string Name { get; set; }

        public string SearchTarget
        {
            get { return _searchTarget; }
            set
            {
                if (value == _searchTarget) return;
                _searchTarget = value;
                OnPropertyChanged();
            }
        }

        public List<string> Tokens
        {
            get { return _tokens; }
            set
            {
                if (Equals(value, _tokens)) return;
                _tokens = value;
                OnPropertyChanged();
            }
        }

        public List<Blight> Blights
        {
            get { return _blights; }
            set
            {
                if (Equals(value, _blights)) return;
                _blights = value;
                OnPropertyChanged();
            }
        }

        public Brush Highlight
        {
            get { return _highlight; }
            set
            {
                if (value.Equals(_highlight)) return;
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
