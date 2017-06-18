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
    public abstract class Hero : ITriggerRegistrar
    {
        private readonly List<IPower> _powerDeck;
        private Stash _stash;
        private readonly Dictionary<string, ITactic> _tactics;
        private readonly Dictionary<string, IAction> _actions;
        private readonly List<IRollModifier> _rollModifiers;
        private List<IRollHandler> _rollHandlers;

        public TriggerRegistry<HeroTrigger> Triggers { get; }

        protected Hero(string name, int defaultGrace, int defaultSecrecy, params IPower[] powers)
        {
            Name = name;
            DefaultGrace = defaultGrace;
            Grace = defaultGrace;
            DefaultSecrecy = defaultSecrecy;
            Secrecy = defaultSecrecy;
            Powers = new List<IPower>();
            _powerDeck = new List<IPower>(powers);
            _stash= new Stash();
            Triggers = new TriggerRegistry<HeroTrigger>(this);
            IsActionAvailable = true;

            var fightTactic = new BasicFightTactic();
            var eludeTactic = new BasicEludeTactic();
            _tactics = new Dictionary<string, ITactic>(StringComparer.InvariantCultureIgnoreCase)
            {
                {fightTactic.Name, fightTactic},
                {eludeTactic.Name, eludeTactic}
            };
            _rollModifiers = new List<IRollModifier>();

            var attack = new Attack();
            _actions = new Dictionary<string, IAction> { {attack.Name, attack}};
            Location = Location.Monastery;
        }

        public IEnumerable<IPower> PowerDeck => _powerDeck;

        public int DefaultGrace { get; set; }
        public int DefaultSecrecy { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; set; }
        public HeroState State { get; set; }
        public ICollection<IPower> Powers { get; protected set; }
        public string Name { get; set; }

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

        public bool IsTurnTaken { get; set; }

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
                this.Death();
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
            throw new NotImplementedException();
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
            return GetAvailableTactics().Where(x=>x.Type == TacticType.Fight ).ToList();
        }

        public List<ITactic> GetAvailableTactics()
        {
            return _tactics.Values.Where(x => x.IsAvailable(this)).ToList();
        }

        public Game Game { get; private set; }
        public IPlayer Player { get; private set; }

        public IEnumerable<Location> GetValidMovementLocations()
        {
            var locations = GetSpace().AdjacentLocations;
            var blocks = _stash.GetAll<PreventMovementEffect>();
            var valid = locations.Where(loc => !blocks.Any(block => block.Matches(loc)));
            return valid;
        }

        public IPower GetPower(string name)
        {
            return Powers.SingleOrDefault(x=>x.Name==name);
        }

        public void RemoveBySource<T>(string name)
        {
            _stash.RemoveBySource<T>(name);
        }

        public IEnumerable<T> GetPowers<T>()
        {
            return Powers.Where(x=>x is T).Cast<T>();
        }

        public void TakeAction(IAction action)
        {
            if (!IsActionAvailable)
                throw new InvalidOperationException($"{Name} does not have an action available.");
            action.Act(this);
        }

        public bool IsActionAvailable { get; set; }
        public ConflictState ConflictState { get; set; }

        public void SelectTactic(string tacticName, ICollection<Blight> targets)
        {
            ValidateState(HeroState.SelectingTarget);
            var targetsAreValid =  ConflictState.AvailableTargets.Intersect(targets).Count() == targets.Count
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
            ConflictState.Roll = roll;
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
            var validRolls = assignments.Select(x => x.DieValue).Intersect(ConflictState.Roll).Count() == assignments.Count;
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
    }
}
