using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class EventVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private Visibility _visibility;
        private string _title;
        private string _text;
        private string _fate;
        private List<EventRowVm> _rows;
        private List<EventOptionVm> _options;

        public EventVm(Game game)
        {
            _game = game;
            Visibility = Visibility.Hidden;
        }

        public EventVm()
        {
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

        public string Fate
        {
            get { return _fate; }
            set
            {
                if (value == _fate) return;
                _fate = value;
                OnPropertyChanged();
            }
        }

        public List<EventRowVm> Rows
        {
            get { return _rows; }
            set
            {
                if (Equals(value, _rows)) return;
                _rows = value;
                OnPropertyChanged();
            }
        }

        public List<EventOptionVm> Options
        {
            get { return _options; }
            set
            {
                if (Equals(value, _options)) return;
                _options = value;
                OnPropertyChanged();
            }
        }

        public void Update(EventModel model)
        {
            if (model == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            Visibility = Visibility.Visible;
            Title = model.Title;
            Text = model.Text;
            Fate = model.Fate > 0 ? model.Fate.ToString() : null;
            Rows = model.Rows.Select(EventRowVm.Create).ToList();
            Options = model.Options
                .Select(o => EventOptionVm.Create(_game, o))
                .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
