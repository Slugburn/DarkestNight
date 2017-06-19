using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes.Impl;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public abstract class Hero
    {
        private readonly Dictionary<string, IAction> _actions;
        private readonly List<IPower> _powerDeck;
        private readonly List<IRollModifier> _rollModifiers;
        private readonly Dictionary<string, ITactic> _tactics;
        private ILocationSelectedHandler _locationSelectedHandler;
        private List<IRollHandler> _rollHandlers;
        private readonly Stash _stash;
        private List<ActionFilter> _actionFilters;

        protected Hero(string name, int defaultGrace, int defaultSecrecy, params IPower[] powers)
        {
            Name = name;
            DefaultGrace = defaultGrace;
            Grace = defaultGrace;
            DefaultSecrecy = defaultSecrecy;
            Secrecy = defaultSecrecy;
            Powers = new List<IPower>();
            _powerDeck = new List<IPower>(powers);
            _stash = new Stash();
            Triggers = new TriggerRegistry<HeroTrigger, Hero>(this);
            IsActionAvailable = true;
            CanGainGrace = true;

            _actions = new IAction[] {new Travel(), new Hide(), new Attack(), new Search(), new Pray()}
                .ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);
            _tactics = new ITactic[] {new BasicFightTactic(), new BasicEludeTactic()}
                .ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);
            _rollModifiers = new List<IRollModifier>();
            _actionFilters = new List<ActionFilter>();

            Location = Location.Monastery;
            TravelSpeed = 1;
        }

        public void AddActionFilter(string name, HeroState state, ICollection<string> allowed)
        {
            var filter = new ActionFilter {Name=name, State=state, Allowed = allowed};
            _actionFilters.Add(filter);
        }

        public void RemoveActionFilter(string name)
        {
            _actionFilters.RemoveAll(x => x.Name == name);
        }

        private class ActionFilter
        {
            public string Name {get;set;}
            public HeroState State { get; set; }
            public ICollection<string> Allowed { get; set; }
        }

        public TriggerRegistry<HeroTrigger, Hero> Triggers { get; }
        public List<int> Roll { get; set; }

        public IEnumerable<IPower> PowerDeck => _powerDeck;

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
        public List<Blight> DefendList { get; set; }

        public int TravelSpeed { get; set; }

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
            Triggers.Handle(HeroTrigger.StartTurn);
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

        public void LoseGrace()
        {
            Grace--;
            if (Grace < 0) Grace = 0;
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
            throw new NotImplementedException();
        }

        public void LoseSecrecy(string sourceName)
        {
            if (!Triggers.Handle(HeroTrigger.LoseSecrecy, sourceName)) return;
            Secrecy--;
            if (Secrecy < 0)
                Secrecy = 0;
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
        }

        public void GainSecrecy(int amount, int max)
        {
            throw new NotImplementedException();
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

        internal void ResolveAttack(Blight blight, int result)
        {
            LoseSecrecy("Attack");
            var space = GetSpace();
            var blightInfo = new BlightFactory().Create(blight);
            if (result < blightInfo.Might)
            {
                if (Triggers.Handle(HeroTrigger.FailedAttack))
                    blightInfo.Defend(this);
            }
            else
            {
                space.RemoveBlight(blight);
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

        public void SelectTactic(string tacticName, ICollection<Blight> targets)
        {
            ValidateState(HeroState.SelectingTarget);
            var targetsAreValid = ConflictState.AvailableTargets.Intersect(targets).Count() == targets.Count
                                  && targets.Count >= ConflictState.MinTarget && targets.Count <= ConflictState.MaxTarget;
            if (!targetsAreValid)
                throw new Exception("Invalid targets.");

            var tacticInfo = ConflictState.AvailableTactics.SingleOrDefault(x => x.Name == tacticName);
            var tacticIsValid = tacticInfo != null;
            if (!tacticIsValid)
                throw new Exception($"Invalid tactic: {tacticName}");

            ConflictState.Targets = targets;
            ConflictState.SelectedTactic = tacticInfo;
            var diceCount = tacticInfo.DiceCount;
            var tactic = _tactics[tacticName];
            tactic.Use(this);
            var roll = Player.RollDice(diceCount).ToList();
            Roll = roll;
            State = HeroState.RollAvailable;
        }

        public void EndCombat()
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

        public void AssignDiceToBlights(ICollection<BlightDieAssignment> assignments)
        {
            ValidateState(HeroState.AssigningDice);
            var validRolls = assignments.Select(x => x.DieValue).Intersect(Roll).Count() == assignments.Count;
            if (!validRolls)
                throw new Exception("Invalid die values specified.");
            var validTargets = assignments.Select(x => x.Blight).Intersect(ConflictState.Targets).Count() == assignments.Count;
            if (!validTargets)
                throw new Exception("Invalid targets specified.");
            foreach (var assignment in assignments)
            {
                ResolveAttack(assignment.Blight, assignment.DieValue);
            }
        }

        public void SetRollHandler(IRollHandler rollHandler)
        {
            _rollHandlers = new List<IRollHandler> {rollHandler};
        }

        public void RemoveRollModifier(string name)
        {
            var toRemove = _rollModifiers.Single(x => x.Name == name);
            _rollModifiers.Remove(toRemove);
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
    }
}