using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.IO;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Wpf.Annotations;
using Slugburn.DarkestNight.Wpf.Commands;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class PlayerVm : IPlayer, INotifyPropertyChanged
    {
        private int _darkness;
        private List<LocationVm> _locations;
        private List<HeroVm> _heroes = new List<HeroVm>();
        private Game _game;
        private ConflictVm _conflict;
        private EventVm _event;
        private SearchVm _search;
        private QuestionVm _question;
        private PowerSelectionVm _powerSelection;
        private PrayerVm _prayer;
        private ICommand _command;
        private NecromancerVm _necromancer;

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
                Necromancer = new NecromancerVm();
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

        public List<LocationVm> Locations
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

        public NecromancerVm Necromancer
        {
            get { return _necromancer; }
            set
            {
                if (Equals(value, _necromancer)) return;
                _necromancer = value;
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

        public PlayerState State { get; set; }
        public void DisplayEvent(EventModel model)
        {
            Event.Update(model);
        }

        public void DisplayConflict(ConflictModel model)
        {
            Conflict.Update(model);
        }

        public Task<string> SelectPower(ICollection<PowerModel> models)
        {
            var source = new TaskCompletionSource<string>();
            PowerSelection.Update(models, source);
            return source.Task;
        }

        public Task<IEnumerable<int>> SelectBlights(BlightSelectionModel model)
        {
            return new SelectBlightsCommand(this, model).Execute();
        }

        public Task<Location> SelectLocation(ICollection<string> locations)
        {
            var source = new TaskCompletionSource<Location>();
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
                    var result = location.Name.ToEnum<Location>();
                    source.SetResult(result);
                });
            }
            return source.Task;
        }

        public void DisplayNecromancer(NecromancerModel model)
        {
            Necromancer.Update(model);
        }

        public Task<Hero> SelectHero(HeroSelectionModel model)
        {
            var source = new TaskCompletionSource<Hero>();
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
                    source.SetResult(selectedHero);
                });
            }
            return source.Task;
        }

        public Task<string> AskQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }

        public void DisplaySearch(SearchModel model, Callback<Find> callback)
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

        public void UpdateBoard(BoardModel model)
        {
            Darkness = model.Darkness;
            Locations = LocationVm.CreateLocations(model.Locations);
        }

        public void UpdateHeroCommands(HeroActionModel model)
        {
            var hero = Heroes.Single(x => x.Name == model.HeroName);
            hero.Commands = CommandVm.Create(Game, model.HeroName, model.Commands);
            hero.Powers = PowerVm.Create(model.Powers);
            hero.Items = ItemVm.Create(model.Items);
        }

        public void UpdateHeroStatus(string heroName, HeroStatusModel status)
        {
            var hero = Heroes.Single(x => x.Name == heroName);
            hero.Status = new HeroStatus(status);
        }

        public void OnNewDay()
        {
            SaveGame();
        }

        private void SaveGame()
        {
            var serializer = new GameSerializer();
            const string path = "game.json";
            if (File.Exists(path))
                File.Delete(path);
            using (var writer = File.CreateText(path))
                serializer.Write(_game, writer);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}