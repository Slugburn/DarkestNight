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
    public class PlayerVm : IPlayer, INotifyPropertyChanged
    {
        private int _darkness;
        private List<Location> _locations;
        private List<HeroVm> _heroes = new List<HeroVm>();
        private Game _game;
        private ConflictVm _conflict;
        private EventVm _event;
        private SearchVm _search;
        private QuestionVm _question;
        private PowerSelectionVm _powerSelection;
        private PrayerVm _prayer;

        public Game Game
        {
            get { return _game; }
            set
            {
                _game = value;
                Conflict = new ConflictVm(_game);
                Event = new EventVm(_game);
                Search = new SearchVm(_game);
                Question = new QuestionVm(_game);
                PowerSelection = new PowerSelectionVm(_game);
                Prayer = new PrayerVm(_game);
            }
        }

        public PrayerVm Prayer
        {
            get { return _prayer; }
            set
            {
                if (Equals(value, _prayer)) return;
                _prayer = value;
                OnPropertyChanged();
            }
        }

        public PowerSelectionVm PowerSelection
        {
            get { return _powerSelection; }
            set
            {
                if (Equals(value, _powerSelection)) return;
                _powerSelection = value;
                OnPropertyChanged();
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

        public List<HeroVm> Heroes
        {
            get { return _heroes; }
            set
            {
                if (Equals(value, _heroes)) return;
                _heroes = value;
                OnPropertyChanged();
            }
        }

        public EventVm Event
        {
            get { return _event; }
            set
            {
                if (Equals(value, _event)) return;
                _event = value;
                OnPropertyChanged();
            }
        }

        public ConflictVm Conflict
        {
            get { return _conflict; }
            private set
            {
                if (Equals(value, _conflict)) return;
                _conflict = value;
                OnPropertyChanged();
            }
        }

        public SearchVm Search
        {
            get { return _search; }
            set
            {
                if (Equals(value, _search)) return;
                _search = value;
                OnPropertyChanged();
            }
        }

        public QuestionVm Question
        {
            get { return _question; }
            set
            {
                if (Equals(value, _question)) return;
                _question = value;
                OnPropertyChanged();
            }
        }

        public PlayerState State { get; set; }
        public void DisplayEvent(EventModel model)
        {
            Event.Update(model);
        }

        public void DisplayConflict(ConflictModel model)
        {
            Conflict.Update(model);
        }

        public void DisplayPowers(ICollection<PowerModel> models, Callback callback)
        {
            PowerSelection.Update(models, callback);
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
                        inner.Highlight = new SolidColorBrush(Colors.White);
                        inner.SelectCommand = null;
                    }
                    callback.Handle(location.Name.ToEnum<Rules.Location>());
                });
            }
        }

        public void DisplayNecromancer(NecromancerModel model, Callback callback)
        {
            callback.Handle(null);
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
                    callback.Handle(selectedHero);
                });
            }
        }

        public void DisplayAskQuestion(QuestionModel model, Callback callback)
        {
            Question.Update(model, callback);
        }

        public void DisplaySearch(SearchModel model, Callback callback)
        {
            Search.Update(model, callback);
        }

        public void DisplayPrayer(PrayerModel model)
        {
            Prayer.Update(model);
        }

        public void AddHero(HeroModel view)
        {
            Heroes.Add(new HeroVm(Game, view));
        }

        public void UpdateBoard(BoardModel view)
        {
            Darkness = view.Darkness;
            Locations = view.Locations.Select(l=>new Location(l)).ToList();
        }

        public void UpdateHeroCommands(HeroActionModel model)
        {
            var hero = Heroes.Single(x => x.Name == model.HeroName);
            hero.Commands = HeroCommand.Create(Game, model.HeroName, model.Commands);
            hero.Powers = HeroPowerVm.Create(model.Powers);
            hero.Items = ItemVm.Create(model.Items);
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