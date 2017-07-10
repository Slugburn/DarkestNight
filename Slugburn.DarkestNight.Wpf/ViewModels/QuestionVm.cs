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
    public class QuestionVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private Visibility _visibility;
        private string _title;
        private string _text;
        private List<QuestionAnswer> _answers;

        public QuestionVm(Game game)
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

        public List<QuestionAnswer> Answers
        {
            get { return _answers; }
            set
            {
                if (Equals(value, _answers)) return;
                _answers = value;
                OnPropertyChanged();
            }
        }

        public void Update(QuestionModel model, Callback<string> callback)
        {
            Visibility = Visibility.Visible;
            Title = model.Title;
            Text = model.Text;
            Answers = model.Answers.Select(answer => CreateQuestionAnswer(answer, callback)).ToList();
        }

        private QuestionAnswer CreateQuestionAnswer(string answer, Callback<string> callback)
        {
            var command = new CommandHandler(() =>
            {
                Visibility = Visibility.Hidden;
                callback.Handle(answer);
            });
            return new QuestionAnswer(answer, command);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class QuestionAnswer
    {
        public QuestionAnswer(string text, ICommand command)
        {
            Text = text;
            Command = command;
        }

        public string Text { get; }
        public ICommand Command { get;  }
    }
}
