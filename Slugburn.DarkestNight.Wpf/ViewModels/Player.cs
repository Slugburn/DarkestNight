using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Player : IPlayer, INotifyPropertyChanged
    {
        private int _darkness;
        private List<Location> _locations;
        private List<Hero> _heroes = new List<Hero>();
        private Game _game;
        private Conflict _conflict;

        public Game Game
        {
            get { return _game; }
            set
            {
                _game = value;
                Conflict = new Conflict(value);
            }
        }

        public int Darkness
        {
            get { return _darkness; }
            set
            {
                if (value == _darkness) return;
                _darkness = value;
                OnPropertyChanged();
            }
        }

        public List<Location> Locations
        {
            get { return _locations; }
            set
            {
                if (Equals(value, _locations)) return;
                _locations = value;
                OnPropertyChanged();
            }
        }

        public List<Hero> Heroes
        {
            get { return _heroes; }
            set
            {
                if (Equals(value, _heroes)) return;
                _heroes = value;
                OnPropertyChanged();
            }
        }

        public Conflict Conflict
        {
            get { return _conflict; }
            private set
            {
                if (Equals(value, _conflict)) return;
                _conflict = value;
                OnPropertyChanged();
            }
        }

        public PlayerState State { get; set; }
        public void DisplayEvent(EventModel playerEvent)
        {
            throw new NotImplementedException();
        }

        public void DisplayConflict(ConflictModel model)
        {
            Conflict.Update(model);
        }

        public void DisplayPowers(ICollection<PowerModel> powers, Callback callback)
        {
            throw new NotImplementedException();
        }

        public void DisplayBlightSelection(BlightSelectionModel blightSelection, Callback callback)
        {
            throw new NotImplementedException();
        }

        public void DisplayLocationSelection(ICollection<string> locations, Callback callback)
        {
            var valid = (from name in locations join loc in Locations on name equals loc.Name select loc).ToList();
            foreach (var location in valid)
            {
                location.Highlight = new SolidColorBrush(Colors.Orange);
                location.SelectCommand = new CommandHandler(() =>
                {
                    foreach (var inner in valid)
                    {
                        inner.Highlight = new SolidColorBrush(Colors.LightGray);
                        inner.SelectCommand = null;
                    }
                    CallbackRouter.Route(Game, callback, location.Name.ToEnum<Rules.Location>());
                });
            }
        }

        public void DisplayNecromancer(NecromancerModel model, Callback callback)
        {
            CallbackRouter.Route(Game, callback, null);
        }

        public void DisplayHeroSelection(HeroSelectionModel model, Callback callback)
        {
            var valid = (from name in model.Heroes join hero in Heroes on name equals hero.Name select hero).ToList();
            foreach (var hero in valid)
            {
                hero.Highlight = new SolidColorBrush(Colors.Orange);
                hero.SelectCommand = new CommandHandler(() =>
                {
                    foreach (var inner in valid)
                    {
                        inner.Highlight = new SolidColorBrush(Colors.White);
                        inner.SelectCommand = null;
                    }
                    var selectedHero = Game.GetHero(hero.Name);
                    CallbackRouter.Route(Game, callback, selectedHero);
                });
            }
        }

        public void DisplayAskQuestion(QuestionModel view, Callback callback)
        {
            throw new NotImplementedException();
        }

        public void DisplaySearch(PlayerSearch view, Callback callback)
        {
            throw new NotImplementedException();
        }

        public void DisplayPrayer(PlayerPrayer view)
        {
            throw new NotImplementedException();
        }

        public void AddHero(HeroModel view)
        {
            Heroes.Add(new Hero(Game, view));
        }

        public void UpdateBoard(BoardModel view)
        {
            Darkness = view.Darkness;
            Locations = view.Locations.Select(l=>new Location(l)).ToList();
        }

        public void UpdateHeroCommands(string heroName, IEnumerable<CommandModel> commands)
        {
            var hero = Heroes.Single(x => x.Name == heroName);
            hero.Commands = commands.Select(c=>new HeroCommand(Game, heroName, c)).ToList();
        }

        public void UpdateHeroStatus(string heroName, HeroStatusModel status)
        {
            var hero = Heroes.Single(x => x.Name == heroName);
            hero.Status = new HeroStatus(status);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}