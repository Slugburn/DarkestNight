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
    public class BlightVm : INotifyPropertyChanged
    {
        private bool _isSelected;
        private bool _isSelectable;
        private Brush _highlight;
        private ICommand _command;

        public static BlightVm Create(BlightModel model)
        {
            return new BlightVm
            {
                Id = model.Id,
                Name = model.Name,
                Effect = model.Effect,
                Might = model.Might,
                Defense = model.Defense
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Effect { get; set; }
        public int Might { get; set; }
        public string Defense { get; set; }

        public Brush Highlight
        {
            get { return _highlight; }
            set
            {
                if (Equals(value, _highlight)) return;
                _highlight = value;
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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelectable
        {
            get { return _isSelectable; }
            set
            {
                if (value == _isSelectable) return;
                _isSelectable = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<BlightVm> CreateBlights(IEnumerable<BlightModel> models)
        {
            return models.Select(Create).ToList();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Clear()
        {
            IsSelected = false;
            IsSelectable = false;
            Highlight = null;
            Command = null;
            PropertyChanged = null;
        }
    }
}