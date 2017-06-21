using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class Hero
    {
        private readonly Dictionary<string, IAction> _actions;
        private readonly List<IPower> _powerDeck;
        private readonly List<IRollModifier> _rollModifiers;
        private readonly Stash _stash;
        private readonly Dictionary<string, ITactic> _tactics;
        private readonly List<ActionFilter> _actionFilters;
        private ILocationSelectedHandler _locationSelectedHandler;
        private List<IRollHandler> _rollHandlers;

        public Hero()
        {
            _powerDeck = new List<IPower>();
            Powers = new List<IPower>();
            _stash = new Stash();
            Triggers = new TriggerRegistry<HeroTrigger, Hero>(this);
            IsActionAvailable = true;
            CanGainGrace = true;

            _actions = new Dictionary<string, IAction>(StringComparer.InvariantCultureIgnoreCase);
            _tactics = new Dictionary<string, ITactic>(StringComparer.InvariantCultureIgnoreCase);
            _rollModifiers = new List<IRollModifier>();
            _actionFilters = new List<ActionFilter>();

            Location = Location.Monastery;
            TravelSpeed = 1;
            Enemies = new List<string>();
        }

        public TriggerRegistry<HeroTrigger, Hero> Triggers { get; }
        public List<int> Roll { get; set; }

        public List<IPower> PowerDeck
        {
            get { return _powerDeck; }
        }

        public int DefaultGrace { get; set; }
        public int DefaultSecrecy { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; set; }
        public HeroState State { get; set; }
        public ICollection<IPower> Powers { get; protected set; }
        public string Name { get; set; }

        public bool IsTurnTaken { get; set; }

        public Game Game { get; private set; }
        public IPlayer Player { get; private set; }

        public bool IsActionAvailable { get; set; }
        public ConflictState ConflictState { get; set; }

        public IList<string> AvailableActions { get; set; }

        public bool CanGainGrace { get; set; }
        public List<string> Enemies { get; set; }

        public int TravelSpeed { get; set; }

        public HeroEvent CurrentEvent { get; set; }

        public int AvailableMovement { get; set; }
        public int FreeActions { get; set; }

        public void AddActionFilter(string name, HeroState state, ICollection<string> allowed)
        {
            var filter = new ActionFilter {Name = name, State = state, Allowed = allowed};
            _actionFilters.Add(filter);
        }

        public void RemoveActionFilter(string name)
        {
            _actionFilters.RemoveAll(x => x.Name == name);
        }

        public ICollection<Blight> GetBlights()
        {
            return GetSpace().Blights;
        }

        public ISpace GetSpace()
        {
            return Game.Board[Location];
        }

        public void StartTurn()
        {
            Triggers.Send(HeroTrigger.StartTurn);
            if (Location == Game.Necromancer.Location)
                LoseSecrecy("Necromancer");
            State = HeroState.ChoosingAction;
            AvailableActions = GetAvailableActions();
        }

        private IList<string> GetAvailableActions()
        {
            var actions = _actions.Values.Where(x => x.IsAvailable(this)).Select(x => x.Name);
            var filtered = _actionFilters.Where(x => x.State == State).Aggregate(actions, (x, filter) => x.Intersect(filter.Allowed));
            return filtered.ToList();
        }

        public void EndTurn()
        {
            if (IsAffectedByBlight(Blight.Spies))
            {
                var space = GetSpace();
                var spies = space.Blights.Where(x => x == Blight.Spies);
                foreach (var spy in spies)
                    LoseSecrecy("Spies");
            }

            IsTurnTaken = true;
        }

        public void LoseTurn()
        {
            throw new NotImplementedException();
        }

        public void ExhaustPowers()
        {
            foreach (var power in Powers)
                power.Exhaust(this);
        }

        public void LoseGrace(int amount = 1)
        {
            Grace = Math.Max(Grace - amount, 0);
        }

        public void SpendGrace(int amount)
        {
            if (Grace - amount < 0)
                throw new Exception("Insufficient Grace.");
            Grace -= amount;
        }

        public void TakeWound()
        {
            if (Grace > 0)
                LoseGrace();
            else
                Death();
        }

        private void Death()
        {
            throw new NotImplementedException();
        }

        public void DrawEvent()
        {
            var eventName = Game.Events.Draw();
            var card = EventFactory.CreateCard(eventName);
            CurrentEvent = card.GetHeroEvent(this);
            Triggers.Send(HeroTrigger.EventDrawn);
            Player.DisplayEvent(Players.Models.PlayerEvent.From(CurrentEvent));
        }

        public void LoseSecrecy(string sourceName)
        {
            LoseSecrecy(1, sourceName);
        }

        public void LoseSecrecy(int amount, string sourceName)
        {
            if (!Triggers.Send(HeroTrigger.LoseSecrecy, sourceName)) return;
            Secrecy = Math.Max(Secrecy - amount, 0);
        }

        public void SpendSecrecy(int amount)
        {
            if (Secrecy - amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            Secrecy -= amount;
        }


        public void ChooseAction()
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T item)
        {
            _stash.Add(item);
        }

        public void Remove<T>(T item)
        {
            _stash.Remove(item);
        }


        public void MoveTo(Location location)
        {
            Location = location;
            Triggers.Send(HeroTrigger.LocationChanged);
        }

        public void GainSecrecy(int amount, int max)
        {
            Secrecy = Math.Min(Secrecy+amount, max);
        }

        public void SetDice(RollType rollType, int count)
        {
            throw new NotImplementedException();
        }

        public void GainGrace(int amount, int max)
        {
            Grace = Math.Min(Grace + amount, max);
        }


        public void LearnPower(string name)
        {
            var power = _powerDeck.SingleOrDefault(x => x.Name == name);
            if (power == null)
                throw new Exception($"The power {name} is not available.");
            power.Learn(this);
            _powerDeck.Remove(power);
            Powers.Add(power);
        }

        public void JoinGame(Game game, IPlayer player)
        {
            Game = game;
            Player = player;
        }

        internal void ResolveAttack(int targetId, int result)
        {
            var targetInfo = ConflictState.SelectedTargets.Single(x => x.Id == targetId);
            var isWin = result >= targetInfo.FightTarget;
            if (isWin)
                Triggers.Send(HeroTrigger.FightWon);
            var isNecromancer = targetInfo.Name == "Necromancer";
            if (isNecromancer)
            {
                Game.Necromancer.ResolveAttack(result, this);
                return;
            }
            LoseSecrecy("Attack");
            var blight = (Blight) Enum.Parse(typeof (Blight), targetInfo.Name);
            var blightInfo = blight.GetDetail();
            if (isWin)
            {
                Game.DestroyBlight(Location, blight);
                Triggers.Send(HeroTrigger.DestroyedBlight);
            }
            else
            {
                if (Triggers.Send(HeroTrigger.BeforeBlightDefends))
                    blightInfo.Defend(this);
            }
        }

        public List<ITactic> GetAvailableFightTactics()
        {
            return GetAvailableTactics().Where(x => x.Type == TacticType.Fight).ToList();
        }

        public List<ITactic> GetAvailableTactics()
        {
            return _tactics.Values.Where(x => x.IsAvailable(this)).ToList();
        }

        public IEnumerable<Location> GetValidMovementLocations()
        {
            var locations = GetSpace().AdjacentLocations;
            var blocks = _stash.GetAll<PreventMovementEffect>();
            var valid = locations.Where(loc => !blocks.Any(block => block.Matches(loc)));
            return valid;
        }

        public IPower GetPower(string name)
        {
            return Powers.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<T> GetPowers<T>()
        {
            return Powers.Where(x => x is T).Cast<T>();
        }

        public void TakeAction(IAction action)
        {
            if (!IsActionAvailable)
                throw new InvalidOperationException($"{Name} does not have an action available.");
            action.Act(this);
        }

        public void SelectTactic(string tacticName, ICollection<int> targetIds)
        {
            ValidateState(HeroState.SelectingTarget);
            var targetCount = targetIds.Count;
            var targetsAreValid = ConflictState.AvailableTargets.Select(x => x.Id).Intersect(targetIds).Count() == targetCount
                                  && targetCount >= ConflictState.MinTarget && targetCount <= ConflictState.MaxTarget;
            if (!targetsAreValid)
                throw new Exception("Invalid targets.");

            var tacticInfo = ConflictState.AvailableTactics.SingleOrDefault(x => x.Name == tacticName);
            var tacticIsValid = tacticInfo != null;
            if (!tacticIsValid)
                throw new Exception($"Invalid tactic: {tacticName}");

            var selectedTargets = from id in targetIds join target in ConflictState.AvailableTargets on id equals target.Id select target;
            ConflictState.SelectedTargets = selectedTargets.ToList();
            ConflictState.SelectedTactic = tacticInfo;
            var diceCount = tacticInfo.DiceCount;
            var tactic = _tactics[tacticName];
            tactic.Use(this);
            var roll = Die.Roll(diceCount).ToList();
            Roll = roll;
            Triggers.Send(HeroTrigger.AfterRoll);
            State = HeroState.RollAvailable;
        }

        public void AcceptRoll()
        {
            ValidateState(HeroState.RollAvailable);
            foreach (var handler in _rollHandlers.ToList())
                handler.HandleRoll(this);
        }

        public void ValidateState(HeroState expected)
        {
            if (State != expected)
                throw new UnexpectedStateException(State, expected);
        }

        public void AddTactic(ITactic tactic)
        {
            _tactics.Add(tactic.Name, tactic);
        }

        public void AddRollModifier(IRollModifier rollModifier)
        {
            _rollModifiers.Add(rollModifier);
        }

        public IEnumerable<IRollModifier> GetRollModifiers()
        {
            return _rollModifiers;
        }

        public void AssignDiceToBlights(ICollection<TargetDieAssignment> assignments)
        {
            ValidateState(HeroState.AssigningDice);
            var validRolls = assignments.Select(x => x.DieValue).Intersect(Roll).Count() == assignments.Count;
            if (!validRolls)
                throw new Exception("Invalid die values specified.");
            var validTargets = assignments.Select(x => x.TargetId).Intersect(ConflictState.SelectedTargets.Select(x => x.Id)).Count() == assignments.Count;
            if (!validTargets)
                throw new Exception("Invalid targets specified.");
            foreach (var assignment in assignments)
            {
                ResolveAttack(assignment.TargetId, assignment.DieValue);
            }
        }

        public void SetRollHandler(IRollHandler rollHandler)
        {
            _rollHandlers = new List<IRollHandler> {rollHandler};
        }

        public void RemoveRollModifiers(string name)
        {
            _rollModifiers.RemoveAll(x => x.Name == name);
        }

        public void AddAction(IAction action)
        {
            _actions.Add(action.Name, action);
        }

        public IAction GetAction(string name)
        {
            if (!_actions.ContainsKey(name))
                throw new Exception($"Unknown action {name} requested.");
            var action = _actions[name];
            return action;
        }

        public void RemoveRollHandler(IRollHandler rollHandler)
        {
            _rollHandlers.Remove(rollHandler);
        }

        public void AddRollHandler(IRollHandler handler)
        {
            _rollHandlers.Add(handler);
        }

        public bool IsAffectedByBlight(Blight blight)
        {
            var blightExists = GetBlights().Any(x => x == blight);
            return blightExists && !IsBlightIgnored(blight);
        }

        public bool IsBlightIgnored(Blight blight)
        {
            return Game.IsBlightIgnored(this, blight);
        }

        public void SetLocationSelectedHandler(ILocationSelectedHandler handler)
        {
            _locationSelectedHandler = handler;
        }

        public void SelectLocation(Location location)
        {
            ValidateState(HeroState.SelectingLocation);
            _locationSelectedHandler.Handle(this, location);
        }

        public bool HasAction(string actionName)
        {
            return _actions.ContainsKey(actionName);
        }

        public void RemoveAction(string name)
        {
            _actions.Remove(name);
        }

        public Dice GetDice(RollType rollType, string baseName, int baseDiceCount)
        {
            var baseDetail = new DiceDetail {Name = baseName, Modifier = baseDiceCount};
            var otherDetails = from rollMod in GetRollModifiers()
                let mod = rollMod.GetModifier(this, rollType)
                where mod != 0
                select new DiceDetail {Name = rollMod.Name, Modifier = mod};
            var details = new[] {baseDetail}.Concat(otherDetails).ToList();
            var dice = new Dice(details);
            return dice;
        }

        public Dice GetSearchDice()
        {
            var dice = GetDice(RollType.Search, "Search", 1);
            return dice;
        }

        public void ResolveEvent(IEventCard e, string option)
        {
            throw new NotImplementedException();
        }

        public void PresentCurrentEvent()
        {
            Player.DisplayEvent(PlayerEvent.From(CurrentEvent));
        }

        public void EndEvent()
        {
            CurrentEvent = null;
        }

        public void RollEventDice(IRollHandler rollHandler)
        {
            var dice = GetDice(RollType.Event, "Event", 1);
            Roll = Die.Roll(dice.Total);
            SetRollHandler(rollHandler);
            State = HeroState.RollAvailable;
        }

        public void FaceEnemy(string enemyName)
        {
            Enemies.Add(enemyName);
            new Defend().Act(this);
        }

        public void DrawSearchResult()
        {
            throw new NotImplementedException();
        }

        public IPower DrawPower()
        {
            return _powerDeck.Draw();
        }

        public void SelectEventOption(string optionCode)
        {
            CurrentEvent.SelectedOption = optionCode;
            if (!Triggers.Send(HeroTrigger.EventOptionSelected))
                return;
            var card = EventFactory.CreateCard(CurrentEvent.Name);
            card.Resolve(this, optionCode);
        }

        public void CancelCurrentEvent()
        {
            CurrentEvent = null;
        }

        public void RefreshPowers()
        {
            foreach (var power in Powers)
                power.Refresh();
        }

        private class ActionFilter
        {
            public string Name { get; set; }
            public HeroState State { get; set; }
            public ICollection<string> Allowed { get; set; }
        }

        public bool HasHolyRelic() => false;

        public bool IsTargetNecromancer()
        {
            var conflictState = ConflictState;
            if (conflictState?.SelectedTargets == null || conflictState.SelectedTargets.Count != 1) return false;
            return conflictState.SelectedTargets.First().Name == "Necromancer";
        }
    }
}