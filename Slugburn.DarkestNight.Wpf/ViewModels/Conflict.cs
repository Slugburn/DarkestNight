using System;
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
    public class Conflict : INotifyPropertyChanged
    {
        private readonly Game _game;
        private CommandHandler _command;
        private string _commandText;
        private string _roll;
        private Tactic _selectedTactic;
        private Target _selectedTarget;
        private IEnumerable<Tactic> _tactics;
        private List<Target> _targets;
        private Visibility _visibility;

        public Conflict(Game game)
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

        public Target SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                if (Equals(value, _selectedTarget)) return;
                _selectedTarget = value;
                OnPropertyChanged();
            }
        }

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

        public Tactic SelectedTactic
        {
            get { return _selectedTactic; }
            set
            {
                if (Equals(value, _selectedTactic)) return;
                _selectedTactic = value;
                OnPropertyChanged();
            }
        }

        public CommandHandler Command
        {
            get { return _command; }
            set
            {
                if (Equals(value, _command)) return;
                _command = value;
                OnPropertyChanged();
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
            Action commandAction;
            if (!model.Roll.Any())
            {
                CommandText = "Roll";
                commandAction = () => _game.ActingHero.SelectTactic(SelectedTactic.Name, new List<int> {SelectedTarget.Id});
            }
            else if (!model.IsRollAccepted)
            {
                CommandText = "Accept Roll";
                commandAction = () => _game.ActingHero.AcceptRoll();
            }
            else
            {
                CommandText = "Accept Result";
                commandAction = () => _game.ActingHero.AcceptConflictResult();
            }
            Command = new CommandHandler(() =>
            {
                Visibility = Visibility.Hidden;
                commandAction();
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}