using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;

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

        public PlayerBlightSelection BlightSelection { get; set; }
        public PlayerConflict Conflict { get; set; }
        public PlayerEvent Event { get; set; }
        public ICollection<PlayerPower> Powers { get; set; }
        public ICollection<string> ValidLocations { get; set; }
        public PlayerNecromancer Necromancer { get; set; }

        public PlayerHeroSelection HeroSelection { get; set; }

        public PlayerState State { get; set; }

        public void DisplayEvent(PlayerEvent playerEvent)
        {
            Event = playerEvent;
        }

        public void DisplayConflict(PlayerConflict conflict)
        {
            Conflict = conflict;
        }

        public void DisplayPowers(ICollection<PlayerPower> powers, Callback callback)
        {
            Powers = powers;
            _callback = callback;
        }

        public void DisplayBlightSelection(PlayerBlightSelection blightSelection, Callback callback)
        {
            BlightSelection = blightSelection;
            _callback = callback;
        }

        public void DisplayLocationSelection(ICollection<string> locations, Callback callback)
        {
            ValidLocations = locations;
            _callback = callback;
        }

        public void DisplayNecromancer(PlayerNecromancer necromancer)
        {
            Necromancer = necromancer;
        }

        public void DisplayHeroSelection(PlayerHeroSelection view, Callback callback)
        {
            HeroSelection = view;
            _callback = callback;
        }

        public void DisplayAskQuestion(PlayerAskQuestion view, Callback callback)
        {
            AskQuestion = view;
            _callback = callback;
        }

        public PlayerAskQuestion AskQuestion { get; set; }


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

        public void TakeAction(string heroName, string actionName)
        {
            var hero = _game.GetHero(heroName);
            var action = hero.GetAction(actionName);
            action.Act(hero);
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
            var power = _game.ActingHero.GetPower(powerName);
            CallbackRouter.Route(_game, _callback, power);
        }

        public void SelectBlight(Location location, Blight blight)
        {
            var data = new[] {new BlightLocation(blight, location)};
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

        public void SelectBlights(string[] blights)
        {
            var hero = _game.ActingHero;
            var blightLocations = blights.Select(b => new BlightLocation(b.ToEnum<Blight>(), hero.Location));
            CallbackRouter.Route(_game, _callback, blightLocations);
        }

        public void SelectHero(string heroName)
        {
            var selectedHero = _game.GetHero(heroName);
            CallbackRouter.Route(_game, _callback, selectedHero);
        }

        public void AnswerQuestion(bool answer)
        {
            CallbackRouter.Route(_game, _callback, answer);
        }
    }
}