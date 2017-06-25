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
        public RollState CurrentRoll { get; set; }

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
        public IPower SelectedPower { get; set; }
        public Queue<string> EventQueue { get; } = new Queue<string>();
        public List<string> Inventory { get; set; } = new List<string>();
        public bool SavedByGrace { get; private set; }

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

        public IList<string> GetAvailableActions()
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
            Triggers.Send(HeroTrigger.EndOfTurn);
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
            {
                LoseGrace();
                SavedByGrace = true;
            }
            else
            {
                Death();
            }
        }

        private void Death()
        {
            throw new NotImplementedException();
        }

        public void DrawEvent()
        {
            var eventName = Game.Events.Draw();
            var card = EventFactory.CreateCard(eventName);
            CurrentEvent = card.Detail.GetHeroEvent(this);
            Triggers.Send(HeroTrigger.EventDrawn);
            Player.State = PlayerState.Event;
            Player.DisplayEvent(PlayerEvent.From(CurrentEvent));
        }

        public void LoseSecrecy(string sourceName)
        {
            LoseSecrecy(1, sourceName);
        }

        public void LoseSecrecy(int amount, string sourceName = null)
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
            var blight = targetInfo.Name.ToEnum<Blight>();
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
            var tactic = _tactics[tacticName];
            tactic.Use(this);

            CurrentRoll.RollType = tactic.GetRollType();
            CurrentRoll.TargetNumber = ConflictState.SelectedTargets.Min(x => tactic.Type == TacticType.Fight ? x.FightTarget : x.EludeTarget);
            CurrentRoll.BaseDiceCount = tactic.GetDiceCount();
            CurrentRoll.BaseName = tactic.Name;

            CurrentRoll.Roll();
        }

        public void AcceptRoll()
        {
            CurrentRoll.Accept();
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

        public void AssignDiceToTargets(ICollection<TargetDieAssignment> assignments)
        {
            ValidateState(HeroState.AssigningDice);
            var validRolls = assignments.Select(x => x.DieValue).Intersect(CurrentRoll.AdjustedRoll).Count() == assignments.Count;
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

        public void DisplayCurrentEvent()
        {
            Player.DisplayEvent(PlayerEvent.From(CurrentEvent));
            Player.State = PlayerState.Event;
        }

        public void EndEvent()
        {
            CurrentEvent = null;
            ContinueTurn();
        }

        private void ContinueTurn()
        {
            // what should go here?
        }

        public void RollEventDice(IRollHandler rollHandler)
        {
            var state = SetRoll(RollBuilder.Create(rollHandler).Type(RollType.Event).Base("Event", 1));
            state.Roll();
            DisplayCurrentEvent();
        }

        public RollState SetRoll(IRollStateCreation creation)
        {
            var rollState = ((RollBuilder.RollStateCreation) creation).Create(this);
            CurrentRoll = rollState;
            return rollState;
        }

        public void FaceEnemy(string enemyName)
        {
            Enemies.Add(enemyName);
            new Defend().Act(this);
        }

        public IPower DrawPower()
        {
            return _powerDeck.Draw();
        }

        public void SelectEventOption(string optionCode)
        {
            var validCodes = CurrentEvent.Options.Select(x=>x.Code).ToList();
            if (!validCodes.Contains(optionCode))
                throw new ArgumentOutOfRangeException(nameof(optionCode), optionCode, $"Valid codes: {validCodes.ToCsv()}");
            CurrentEvent.SelectedOption = optionCode;
            if (!Triggers.Send(HeroTrigger.EventOptionSelected))
                return;
            var card = EventFactory.CreateCard(CurrentEvent.Name);
            // An event can't be ignored after it has been resolved
            CurrentEvent.IsIgnorable = false;
            card.Resolve(this, optionCode);
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

        public void DrawSearchResult()
        {
            var map = Game.Maps.Draw();
            var space = GetSpace();
            var result = map.GetSearchResult(space.Location);
            switch (result)
            {
                case Find.Key:
                    Inventory.Add("Key");
                    break;
                case Find.BottledMagic:
                    Inventory.Add("Bottled Magic");
                    break;
                case Find.SupplyCache:
                    break;
                case Find.TreasureChest:
                    Inventory.Add("Treasure Chest");
                    break;
                case Find.Waystone:
                    Inventory.Add("Waystone");
                    break;
                case Find.ForgottenShrine:
                    break;
                case Find.VanishingDust:
                    Inventory.Add("Vanishing Dust");
                    break;
                case Find.Epiphany:
                    break;
                case Find.Artifact:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}