using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class LocationVm : INotifyPropertyChanged
    {
        private string _searchTarget;
        private List<string> _tokens;
        private List<BlightVm> _blights = new List<BlightVm>();
        private Brush _highlight = new SolidColorBrush(Colors.White);
        private ICommand _selectCommand;
        private Visibility _relicVisiblity;

        private static LocationVm Create(LocationModel model, IEnumerable<HeroVm> heroes)
        {
            var vm = new LocationVm
            {
                Name = model.Name,
                SearchTarget = model.SearchTarget != null ? $"{model.SearchTarget}+" : null,
                Tokens = model.Tokens,
                Blights = BlightVm.CreateBlights(model.Blights),
                RelicVisiblity = model.HasRelic ? Visibility.Visible : Visibility.Collapsed,
                Heroes = heroes.ToList()
            };
            return vm;
        }

        public List<HeroVm> Heroes { get; set; } = new List<HeroVm>();

        public static List<LocationVm> Create(IEnumerable<LocationModel> locationModels, List<HeroVm> heroes)
        {
            return locationModels.Select(model => Create(model, heroes.Where(x=>x.Status.Location == model.Name))).ToList();
        }
        
        public Visibility RelicVisiblity
        {
            get { return _relicVisiblity; }
            set
            {
                if (value == _relicVisiblity) return;
                _relicVisiblity = value;
                OnPropertyChanged();
            }
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
