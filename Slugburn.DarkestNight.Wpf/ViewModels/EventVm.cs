using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class EventOption
    {
        public EventOption(Game game, EventOptionModel model)
        {
            Text = model.Text;
            Command = new CommandHandler(
                () => game.ActingHero.SelectEventOption(model.Code));
        }


        public static EventOption Create(Game game, EventOptionModel model)
        {
            return new EventOption(game, model);
        }

        public string Text { get; set; }

        public ICommand Command { get; set; }
    }

    public class EventRow
    {
        private EventRow(EventRowModel model)
        {
            Text = model.Text;
            SubText = model.SubText;
            if (model.Max == int.MaxValue)
                Range = $"{model.Min}+";
            else if (model.Min == model.Max)
                Range = model.Min.ToString();
            else
                Range = $"{model.Min} - {model.Max}";
            Highlight = model.IsActive ? new SolidColorBrush(Colors.Yellow) : null;
        }

        public static EventRow Create(EventRowModel model)
        {
            return new EventRow(model);
        }

        public string Range { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public Brush Highlight { get; set; }
    }

    public class EventVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private Visibility _visibility;
        private string _title;
        private string _text;
        private string _fate;
        private List<EventRow> _rows;
        private List<EventOption> _options;

        public EventVm(Game game)
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

        public List<EventRow> Rows
        {
            get { return _rows; }
            set
            {
                if (Equals(value, _rows)) return;
                _rows = value;
                OnPropertyChanged();
            }
        }

        public List<EventOption> Options
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
            Rows = model.Rows.Select(EventRow.Create).ToList();
            Options = model.Options
                .Select(o => EventOption.Create(_game, o))
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
