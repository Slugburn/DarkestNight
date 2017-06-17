using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
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
        private readonly Dictionary<string, ITactic> _fightTactics;
        private readonly Dictionary<string, ITactic> _evadeTactics;
        private readonly List<IRollModifier> _rollModifiers;
        private IRollClient _rollClient;

        public TriggerRegistry<HeroTrigger> Triggers { get; }

        protected Hero(string name, int defaultGrace, int defaultSecrecy, params IPower[] powers)
        {
            Name = name;
            DefaultGrace = defaultGrace;
            Grace = defaultGrace;
            DefaultSecrecy = defaultSecrecy;
            Secrecy = defaultSecrecy;
            foreach (var power in powers.Cast<Power>())
            {
                power.Hero = this;
            }
            Powers = new List<IPower>();
            _powerDeck = new List<IPower>(powers);
            _stash= new Stash();
            Triggers = new TriggerRegistry<HeroTrigger>(this);
            IsActionAvailable = true;

            _fightTactics = new Dictionary<string, ITactic>(StringComparer.InvariantCultureIgnoreCase) {{"None", new NoTactic()}};
            _evadeTactics = new Dictionary<string, ITactic>(StringComparer.InvariantCultureIgnoreCase) {{"None", new NoTactic()}};
            _rollModifiers = new List<IRollModifier>();
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

        public IEnumerable<IBlight> GetBlights()
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
            var space = GetSpace();
            var blights = space.Blights.ToList();
            var spies = space.GetBlights<Spies>();
            foreach (var spy in spies)
            {
                if (!IsIgnoringBlight(spy))
                    LoseSecrecy(spy.Name);
            }

            IsTurnTaken = true;
        }

        private bool IsIgnoringBlight(IBlight blight)
        {
            var effects = _stash.GetAll<IgnoreBlightEffect>();
            return effects.Any(x => x.Match(blight));
        }

        public bool IsTurnTaken { get; set; }

        public void LoseTurn()
        {
            throw new System.NotImplementedException();
        }

        public void ExhaustPowers()
        {
            foreach (var power in Powers)
                power.Exhaust();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new NotImplementedException();
        }


        public IEnumerable<int> GetLastRoll(RollType rollType)
        {
            throw new NotImplementedException();
        }

        public void LearnPower(string name)
        {
            var power = _powerDeck.SingleOrDefault(x => x.Name == name);
            if (power == null)
                throw new Exception($"The power {name} is not available.");
            power.Learn();
            _powerDeck.Remove(power);
            Powers.Add(power);
        }

        public void JoinGame(Game game, IPlayer player)
        {
            Game = game;
            Player = player;
        }

        internal void ResolveAttack(IBlight blight, int result)
        {
            LoseSecrecy("Attack");
            var space = GetSpace();
            if (result < blight.Might)
            {
                if (Triggers.Handle(HeroTrigger.FailedAttack))
                    blight.Defend(this);
            }
            else
            {
                space.RemoveBlight(blight);
            }
        }

        public List<ITactic> GetAvailableFightTactics()
        {
            return _fightTactics.Values.Where(x => x.IsAvailable(this)).ToList();
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

        public ITriggerHandler GetTriggerHandler(string handlerName)
        {
            return (ITriggerHandler) GetPower(handlerName);
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
            var success = action.Act();
            if (success)
                IsActionAvailable = false;
        }

        public bool IsActionAvailable { get; private set; }
        public ConflictState ConflictState { get; set; }

        public void SelectTargetAndTactic(ICollection<Blight> targets, string tacticName)
        {
            ValidateState(HeroState.SelectingTarget);
            var targetsAreValid =  ConflictState.AvailableTargets.Intersect(targets).Count() == targets.Count
                && targets.Count >= ConflictState.MinTarget && targets.Count <= ConflictState.MaxTarget;
            if (!targetsAreValid)
                throw new Exception("Invalid targets.");
            var tacticIsValid = ConflictState.AvailableFightTactics.Select(x=>x.Name).Contains(tacticName);
            if (!tacticIsValid)
                throw new Exception($"Invalid tactic: {tacticName}");
            ConflictState.Targets = targets;
            var tacticInfo = ConflictState.AvailableFightTactics.Single(x=>x.Name == tacticName);
            ConflictState.SelectedTactic = tacticInfo;
            var diceCount = tacticInfo.DiceCount;
            var roll = Player.RollDice(diceCount).ToList();
            ConflictState.Roll = roll;
            State = HeroState.RollAvailable;
        }

        public void EndCombat()
        {
            ValidateState(HeroState.RollAvailable);
            var tactic = _fightTactics[ConflictState.SelectedTactic.Name];
            var roll = ConflictState.Roll;
            tactic.AfterRoll(this, roll);
            _rollClient.EndCombat(roll);
        }

        private void ValidateState(HeroState expected)
        {
            if (State != expected)
                throw new UnexpectedStateException(State, expected);
        }

        public void AddFightTactic(ITactic tactic)
        {
            _fightTactics.Add(tactic.Name, tactic);
        }

        public void AddEvadeTactic(ITactic tactic)
        {
            _evadeTactics.Add(tactic.Name, tactic);
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
                ResolveAttack(GetSpace().GetBlight(assignment.Blight) , assignment.DieValue);
            }
        }

        public void SetRollClient(IRollClient rollClient)
        {
            _rollClient = rollClient;
        }

        public void RemoveRollModifier(string name)
        {
            var toRemove = _rollModifiers.Single(x => x.Name == name);
            _rollModifiers.Remove(toRemove);
        }
    }

    public class UnexpectedStateException : Exception
    {
        public UnexpectedStateException(HeroState actual, HeroState expected)
            :base($"Unexpected game state. Expected {expected} but was {actual}.")
        {
        }
    }
}
