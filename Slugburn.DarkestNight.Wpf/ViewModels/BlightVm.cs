using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class BlightVm : INotifyPropertyChanged
    {
        private bool _isSelected;

        public BlightVm(BlightModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Effect = model.Effect;
            Might = model.Might;
            Defense = model.Defense;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Effect { get; set; }
        public int Might { get; set; }
        public string Defense { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<BlightVm> CreateBlights(IEnumerable<BlightModel> models)
        {
            return models.Select(b => new BlightVm(b)).ToList();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}