using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Tests.Fakes
{
    public class FakePlayer : IPlayer
    {
        private readonly Game _game;

        private Callback _callback;

        public FakePlayer(Game game)
        {
            _game = game;
        }

        public BlightSelectionModel BlightSelection { get; set; }
        public ConflictModel Conflict { get; set; }
        public EventModel Event { get; set; }
        public ICollection<PowerModel> Powers { get; set; }
        public ICollection<string> ValidLocations { get; set; }
        public NecromancerModel Necromancer { get; set; }

        public HeroSelectionModel HeroSelection { get; set; }

        public PlayerState State { get; set; }

        public void DisplayEvent(EventModel playerEvent)
        {
            Event = playerEvent;
        }

        public void DisplayConflict(ConflictModel conflict)
        {
            Conflict = conflict;
        }

        public void DisplayPowers(ICollection<PowerModel> powers, Callback callback)
        {
            Powers = powers;
            _callback = callback;
        }

        public void DisplayBlightSelection(BlightSelectionModel blightSelection, Callback callback)
        {
            BlightSelection = blightSelection;
            _callback = callback;
        }

        public void DisplayLocationSelection(ICollection<string> locations, Callback callback)
        {
            ValidLocations = locations;
            _callback = callback;
        }

        public void DisplayNecromancer(NecromancerModel model, Callback callback)
        {
            Necromancer = model;
        }

        public void DisplayHeroSelection(HeroSelectionModel model, Callback callback)
        {
            HeroSelection = model;
            _callback = callback;
        }

        public void DisplayAskQuestion(QuestionModel view, Callback callback)
        {
            AskQuestion = view;
            _callback = callback;
        }

        public void DisplaySearch(PlayerSearch view, Callback callback)
        {
            Search = view;
            _callback = callback;
        }

        public void DisplayPrayer(PlayerPrayer view)
        {
            Prayer = view;
        }

        public void AddHero(HeroModel view)
        {
            Heroes.Add(view);
        }

        public void UpdateBoard(BoardModel view)
        {
            Board = view;
        }

        public void UpdateHeroCommands(string heroName, IEnumerable<CommandModel> commands)
        {
            Heroes.Single(x => x.Name == heroName).Commands = commands.ToList();
        }

        public void UpdateHeroStatus(string heroName, HeroStatusModel status)
        {
            Heroes.Single(x => x.Name == heroName).Status = status;
        }

        public QuestionModel AskQuestion { get; set; }
        public PlayerSearch Search { get; set; }
        public PlayerPrayer Prayer { get; set; }
        public List<HeroModel> Heroes { get; set; } = new List<HeroModel>();
        public BoardModel Board { get; set; }
        
        public void SelectEventOption(string option)
        {
            var matching = Event.Options.SingleOrDefault(x => x.Text == option);
            matching.ShouldNotBeNull($"No matching event with Text '{option}'.");
            var code = matching?.Code ?? "<unknown>";
            _game.ActingHero.SelectEventOption(code);
        }

        public void AcceptRoll()
        {
            _game.ActingHero.AcceptRoll();
        }

        public void ExecuteCommand(string heroName, string commandName)
        {
            var hero = _game.GetHero(heroName);
            hero.ExecuteCommand(commandName);
        }

        public void SelectLocation(Location location)
        {
            CallbackRouter.Route(_game, _callback, location);
        }

        public void ResolveConflict( string tacticName, ICollection<int> targetIds)
        {
            var hero = _game.ActingHero;
            hero.SelectTactic(tacticName, targetIds);
        }

        public void SelectPower(string powerName)
        {
            CallbackRouter.Route(_game, _callback, powerName);
        }

        public void SelectBlight(int blightId)
        {
            var data = new[] {blightId};
            CallbackRouter.Route(_game, _callback, data);
        }

        public void FinishNecromancerTurn()
        {
            _game.Necromancer.CompleteTurn();
        }

        public void AssignDiceToTargets(List<TargetDieAssignment> diceAssignment)
        {
            var hero = _game.ActingHero;
            hero.AssignDiceToTargets(diceAssignment);
        }

        public void AcceptConflictResult()
        {
            var hero = _game.ActingHero;
            hero.AcceptConflictResult();
        }

        public void SelectBlights(IEnumerable<int> blightIds)
        {
            CallbackRouter.Route(_game, _callback, blightIds);
        }

        public void SelectHero(string heroName)
        {
            var selectedHero = _game.GetHero(heroName);
            CallbackRouter.Route(_game, _callback, selectedHero);
        }

        public void AnswerQuestion(string answer)
        {
            CallbackRouter.Route(_game, _callback, answer);
        }

        public void SelectSearchResult(string resultName)
        {
            CallbackRouter.Route(_game, _callback, resultName.ToFind());
        }

        public void TradeItem(int itemId, string fromHeroName, string toHeroName)
        {
            var fromHero = _game.GetHero(fromHeroName);
            var toHero = _game.GetHero(toHeroName);
            fromHero.TradeItemTo(itemId, toHero);
        }
    }
}