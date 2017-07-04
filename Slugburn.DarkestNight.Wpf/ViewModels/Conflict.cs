using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Conflict : INotifyPropertyChanged
    {
        private readonly Game _game;
        private string _roll;
        private IEnumerable<Tactic> _tactics;
        private List<Target> _targets;
        private Visibility _rollCommandVisibility;
        private Visibility _acceptRollCommandVisibility;
        private Visibility _visibility;
        private Visibility _acceptResultCommandVisibility;

        public Conflict(Game game)
        {
            _game = game;
            RollCommand = new CommandHandler(RollDice);
            AcceptRollCommand = new CommandHandler(AcceptRoll);
            AcceptResultCommand = new CommandHandler(AcceptResult);
            Visibility = Visibility.Hidden;
        }

        public CommandHandler AcceptResultCommand { get; }

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
                if (Equals(value, _roll)) return;
                _roll = value;
                OnPropertyChanged();
            }
        }

        public List<Target> Targets
        {
            get { return _targets; }
            set
            {
                if (Equals(value, _targets)) return;
                _targets = value;
                OnPropertyChanged();
            }
        }

        public Target SelectedTarget { get; set; }

        public IEnumerable<Tactic> Tactics
        {
            get { return _tactics; }
            set
            {
                if (Equals(value, _tactics)) return;
                _tactics = value;
                OnPropertyChanged();
            }
        }

        public Tactic SelectedTactic { get; set; }

        public ICommand RollCommand { get; }
        public ICommand AcceptRollCommand { get; }

        public Visibility RollCommandVisibility
        {
            get { return _rollCommandVisibility; }
            set
            {
                if (value == _rollCommandVisibility) return;
                _rollCommandVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility AcceptRollCommandVisibility
        {
            get { return _acceptRollCommandVisibility; }
            set
            {
                if (value == _acceptRollCommandVisibility) return;
                _acceptRollCommandVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility AcceptResultCommandVisibility
        {
            get { return _acceptResultCommandVisibility; }
            set
            {
                if (value == _acceptResultCommandVisibility) return;
                _acceptResultCommandVisibility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(ConflictModel model)
        {
            Visibility = model == null ? Visibility.Hidden : Visibility.Visible;
            if (model == null) return;
            Targets = model.Targets.Select(m => new Target(m)).ToList();
            SelectedTarget = Targets.First();
            Tactics = model.Tactics.Select(m => new Tactic(m)).ToList();
            SelectedTactic = Tactics.First();
            Roll = model.Roll.Select(x => x.ToString()).ToCsv();
            RollCommandVisibility = model.Roll.Any() ? Visibility.Hidden : Visibility.Visible;
            AcceptRollCommandVisibility = model.Roll.Any() && !model.IsRollAccepted ? Visibility.Visible : Visibility.Hidden;
            AcceptResultCommandVisibility = model.IsRollAccepted ? Visibility.Visible : Visibility.Hidden;
        }

        private void RollDice()
        {
            _game.ActingHero.SelectTactic(SelectedTactic.Name, new List<int> {SelectedTarget.Id});
        }

        private void AcceptRoll()
        {
            _game.ActingHero.AcceptRoll();
        }

        private void AcceptResult()
        {
            _game.ActingHero.AcceptConflictResult();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}