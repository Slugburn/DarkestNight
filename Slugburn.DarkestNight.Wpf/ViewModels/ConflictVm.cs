using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class ConflictVm : INotifyPropertyChanged
    {
        private readonly Game _game;
        private CommandHandler _command;
        private string _commandText;
        private IEnumerable<int> _roll;
        private TacticVm _selectedTactic;
        private IEnumerable<TacticVm> _tactics;
        private List<TargetVm> _targets;
        private Visibility _visibility;
        private SelectionMode _targetSelectionMode;
        private List<ConflictTargetVm> _selectedTargets;

        public ConflictVm(Game game)
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

        public IEnumerable<int> Roll
        {
            get { return _roll; }
            set
            {
                if (Equals(value, _roll)) return;
                _roll = value;
                OnPropertyChanged();
            }
        }

        public List<TargetVm> Targets
        {
            get { return _targets; }
            set
            {
                if (Equals(value, _targets)) return;
                _targets = value;
                OnPropertyChanged();
            }
        }

        public List<ConflictTargetVm> SelectedTargets
        {
            get { return _selectedTargets; }
            set
            {
                if (Equals(value, _selectedTargets)) return;
                _selectedTargets = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TacticVm> Tactics
        {
            get { return _tactics; }
            set
            {
                if (Equals(value, _tactics)) return;
                _tactics = value;
                OnPropertyChanged();
            }
        }

        public TacticVm SelectedTactic
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

        public SelectionMode TargetSelectionMode
        {
            get { return _targetSelectionMode; }
            set
            {
                if (value == _targetSelectionMode) return;
                _targetSelectionMode = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(ConflictModel model)
        {
            Visibility = model == null ? Visibility.Hidden : Visibility.Visible;
            if (model == null) return;
            Targets = TargetVm.Create(model.Targets);
            if (Targets != null)
                Targets.First().IsSelected = true;
            TargetSelectionMode = model.TargetCount == 1 ? SelectionMode.Single : SelectionMode.Multiple;
            SelectedTargets = ConflictTargetVm.Create(model.SelectedTargets);
            Tactics = TacticVm.CreateTactics(model);
            SelectedTactic = model.SelectedTactic != null
                ? new TacticVm(model.SelectedTactic)
                : Tactics.First();

            Roll = model.Roll;
            if (Targets != null && SelectedTargets == null)
            {
                CommandText = "Roll";
                Func<bool> canExecute = () => Targets.Count(t => t.IsSelected) == model.TargetCount;
                Command = new CommandHandler(SelectTargetAndTactic, canExecute);
                foreach (var target in Targets)
                    target.PropertyChanged += (sender, e) => Command.OnCanExecuteChanged();
            }
            else
            {
                CommandText = "Accept Roll";
                Command = new CommandHandler(() =>
                {
                    Visibility = Visibility.Hidden;
                    _game.ActingHero.AcceptRoll();
                });
            }
        }

        private void SelectTargetAndTactic()
        {
            Visibility = Visibility.Hidden;
            var targetIds = Targets.Where(t => t.IsSelected).Select(t => t.Id).ToList();
            _game.ActingHero.SelectTactic(SelectedTactic.Name, targetIds);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}