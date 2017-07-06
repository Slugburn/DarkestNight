using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class SearchVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private Visibility _visibility;
        private string _roll;
        private List<SearchResult> _results;
        private SearchResult _selectedResult;
        private ICommand _command;
        private Visibility _resultsVisibility;
        private string _commandText;

        public SearchVm(Game game)
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

        public string Roll
        {
            get { return _roll; }
            set
            {
                if (value == _roll) return;
                _roll = value;
                OnPropertyChanged();
            }
        }

        public List<SearchResult> Results
        {
            get { return _results; }
            set
            {
                if (Equals(value, _results)) return;
                _results = value;
                OnPropertyChanged();
            }
        }

        public SearchResult SelectedResult
        {
            get { return _selectedResult; }
            set
            {
                if (value == _selectedResult) return;
                _selectedResult = value;
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

        public void Update(SearchModel model, Callback callback)
        {
            if (model == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            Visibility = Visibility.Visible;
            Roll = model.Roll.Select(x => x.ToString()).ToCsv();
            Results = SearchResult.Create(model.SearchResults);
            var haveResults = Results?.Any() ?? false;
            ResultsVisibility = haveResults ? Visibility.Visible : Visibility.Hidden;
            if (!haveResults)
            {
                CommandText = "Accept Roll";
                Command = new CommandHandler(() => _game.ActingHero.AcceptRoll());
            }
            else
            {
                SelectedResult = Results.First();
                CommandText = "Accept Results";
                Command = new CommandHandler(() =>
                {
                    callback.Handle(SelectedResult.Code);
                });
            }
        }

        public string CommandText
        {
            get { return _commandText; }
            set
            {
                if (value == _commandText) return;
                _commandText = value;
                OnPropertyChanged();
            }
        }

        public Visibility ResultsVisibility
        {
            get { return _resultsVisibility; }
            set
            {
                if (value == _resultsVisibility) return;
                _resultsVisibility = value;
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

    public class SearchResult
    {

        public static List<SearchResult> Create(IEnumerable<SearchResultModel> models)
        {
            return models?.Select(Create).ToList();
        }

        public static SearchResult Create(SearchResultModel model)
        {
            return new SearchResult
            {
                Code = model.Code,
                Name = model.Name,
                Text = model.Text
            };
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
