using System.ComponentModel;
using System.Runtime.CompilerServices;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class TargetVm : INotifyPropertyChanged
    {
        private bool _isSelected;

        public TargetVm(TargetModel model)
        {
            Id = model.Id;
            Name = model.Name;
            TargetNumber = model.TargetNumber?.ToString() ?? $"Fight {model.FightTarget} / Elude {model.EludeTarget}";
            ResultNumber = model.Result;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TargetNumber { get; set; }
        public int? ResultNumber { get; set; }

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}