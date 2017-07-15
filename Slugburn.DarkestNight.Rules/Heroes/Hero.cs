using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.IO;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Powers.Priest;
using Slugburn.DarkestNight.Rules.Powers.Prince;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Spaces;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class Hero
    {
        private readonly Dictionary<string, ICommand> _commands;
        private List<string> _powerDeck;
        private readonly List<IModifier> _modifiers = new List<IModifier>();
        private readonly List<IRollModifier> _rollModifiers = new List<IRollModifier>();
        private readonly Stash _stash;
        private readonly Dictionary<string, ITactic> _tactics;
        private readonly List<IActionFilter> _actionFilters;
        private bool _hasFreeAction;
        private IActionFilter _freeActionFilter;
        private bool _isActionAvailable;
        private bool _movedDuringTurn;
        private bool _endingTurn;
        private int _baseDefaultGrace;

        public Hero()
        {
            _powerDeck = new List<string>();
            Powers = new List<IPower>();
            _stash = new Stash();
            Triggers = new TriggerRegistry<HeroTrigger, Hero>(this);
            IsActionAvailable = true;
            IsGraceGainBlocked = false;

            _commands = new Dictionary<string, ICommand>(StringComparer.InvariantCultureIgnoreCase);
            _tactics = new Dictionary<string, ITactic>(StringComparer.InvariantCultureIgnoreCase);
            _actionFilters = new List<IActionFilter>();

            Location = Location.Monastery;
            TravelSpeed = 1;
            Enemies = new List<IEnemy>();
        }

        public HeroState State { get; set; }

        private bool LoseNextTurn { get; set; }
        public TriggerRegistry<HeroTrigger, Hero> Triggers { get; }
        public RollState CurrentRoll { get; set; }

        public List<string> PowerDeck => _powerDeck;

        public int DefaultGrace
        {
            get { return this.GetModifiedTotal(_baseDefaultGrace, ModifierType.DefaultGrace); }
            set { _baseDefaultGrace = value; }
        }

        public int DefaultSecrecy { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; internal set; }
        public ICollection<IPower> Powers { get; protected set; }
        public string Name { get; set; }

        public bool HasTakenTurn { get; set; }

        public Game Game { get; private set; }
        public IPlayer Player { get; private set; }

        public bool IsActionAvailable
        {
            get
            {
                return _isActionAvailable || _hasFreeAction;
            }
            set
            {
                if (!value && _hasFreeAction)
                {
                    _hasFreeAction = false;
                }
                else
                {
                    _isActionAvailable = value;
                }
            }
        }

        public ConflictState ConflictState { get; set; }

        public IList<ICommand> AvailableCommands { get; set; }

        public bool CanGainGrace()
        {
            if (IsGraceGainBlocked) return false;
            // Taints prevent gaining Grace
            return !IsAffectedByBlight<Taint>();
        }

        public bool IsGraceGainBlocked { get; set; }

        public List<IEnemy> Enemies { get; set; }

        public int TravelSpeed { get; set; }

        public HeroEvent CurrentEvent { get; set; }

        public int AvailableMovement { get; set; }
        public Queue<string> EventQueue { get; } = new Queue<string>();
        private List<IItem> Inventory { get; } = new List<IItem>();
        public bool SavedByGrace { get; private set; }

        public bool IsTakingTurn => this == Game.ActingHero;

        public void AddActionFilter(IActionFilter filter)
        {
            _actionFilters.Add(filter);
        }

        public void RemoveActionFilter(string name)
        {
            _actionFilters.RemoveAll(x => x.Name == name);
        }

        public ICollection<IBlight> GetBlights()
        {
            return Space.Blights;
        }

        public Space Space => Game.Board[Location];

        private IList<ICommand> GetAvailableCommands()
        {
            var heroCommands = _commands.Values;
            var locationCommands = Space.GetActions();
            var itemCommands = GetLocationInventory().OfType<ICommand>();
            var actions = heroCommands.Concat(locationCommands).Concat(itemCommands)
                .Where(x => x.IsAvailable(this));

            // use a free action if we've got one
            if (HasFreeAction)
            {
                if (_freeActionFilter == null) return actions.ToList();
                return actions
                    .Where(_freeActionFilter.IsAllowed)
                    .Concat(new [] { _commands["Skip Free Action"] })
                    .ToList();
            }
            var filtered = _actionFilters.Aggregate(actions, (x, filter) => x.Where(filter.IsAllowed));
            return filtered.ToList();
        }

        public void LoseTurn()
        {
            LoseNextTurn = true;
        }

        public void ExhaustPowers()
        {
            foreach (var power in Powers)
                power.Exhaust(this);
        }

        public async void LoseGrace(int amount = 1, bool canIntercede = true)
        {
            if (amount == 0) return;
            if (Intercession !=  null)
            {
                var interceded = canIntercede && await Intercession.IntercedeForLostGrace(this, amount);
                if (interceded) return;
            }
            var before = Grace;
            Grace = Math.Max(Grace - amount, 0);
            if (Grace == before) return;
            UpdateHeroStatus();
            UpdateAvailableCommands();
        }

        public async void SpendGrace(int amount, bool canIntercede = true)
        {
            if (Intercession != null)
            {
                var interceded = canIntercede && await Intercession.IntercedeForSpentGrace(this, amount);
                if (interceded) return;
            }
            if (Grace - amount < 0)
                throw new Exception("Insufficient Grace.");
            Grace -= amount;
            UpdateHeroStatus();
            UpdateAvailableCommands();
        }

        private IItem GetItem(int itemId)
        {
            var item = Inventory.SingleOrDefault(x => x.Id == itemId);
            if (item == null)
                throw new ArgumentOutOfRangeException(nameof(itemId), itemId, "Unknown item.");
            return item;
        }

        public void TakeWound()
        {
            if (CanSpendGrace)
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
            DoEvent(eventName);
        }

        private void DoEvent(string eventName)
        {
            State = HeroState.ResolvingEvent;
            var card = EventFactory.CreateCard(eventName);
            CurrentEvent = card.Detail.GetHeroEvent(this);
            Game.Triggers.Send(GameTrigger.EventDrawn);
            State = HeroState.EventDrawn;
            UpdateAvailableCommands();
            DisplayCurrentEvent();
        }

        public void LoseSecrecy(string sourceName)
        {
            LoseSecrecy(1, sourceName);
        }

        public void LoseSecrecy(int amount, string sourceName = null)
        {
            if (amount == 0) return;
            if (!Triggers.Send(HeroTrigger.LosingSecrecy, sourceName)) return;
            Secrecy = Math.Max(Secrecy - amount, 0);
            UpdateHeroStatus();
        }

        public void SpendSecrecy(int amount)
        {
            if (Secrecy - amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            Secrecy -= amount;
            UpdateHeroStatus();
            UpdateAvailableCommands();
        }

        public void ChooseAction()
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T item)
        {
            _stash.Add(item);
        }
        
        public void MoveTo(Location location)
        {
            if (!Triggers.Send(HeroTrigger.Moving))
                return;
            Location = location;
            var curses = GetBlights().Count(b => b is Curse && !Game.IsBlightSupressed(b, this));
            if (curses > 0)
                LoseGrace(curses);
            _movedDuringTurn = true;
            UpdateHeroStatus();
            UpdateAvailableCommands();
            Game.UpdatePlayerBoard();
            Triggers.Send(HeroTrigger.Moved);
        }

        public void GainSecrecy(int amount, int max)
        {
            if (Secrecy < max)
                Secrecy = Math.Min(Secrecy+amount, max);
            UpdateHeroStatus();
        }

        public void GainGrace(int amount, int max)
        {
            if (!CanGainGrace()) return;
            if (Grace >= max) return;
            Grace = Math.Min(Grace + amount, max);
            UpdateHeroStatus();
        }

        public void UpdateHeroStatus()
        {
            Player.UpdateHeroStatus(Name, HeroStatusModel.FromHero(this));
        }

        public async void DrawPower()
        {
            var powerName = PowerDeck.First();
            var power = LearnPower(powerName);
            Player.State = PlayerState.SelectPower;
            await Player.SelectPower(new[] {power}.ToPlayerPowers());
            ContinueTurn();
        }

        public IPower LearnPower(string name)
        {
            var powerName = _powerDeck.SingleOrDefault(x => x == name);
            if (powerName == null)
                throw new Exception($"The power {name} is not available.");
            var power = PowerFactory.Create(powerName);
            LearnPower(power);
            _powerDeck.Remove(powerName);
            return power;
        }

        public void LearnPower(IPower power)
        {
            power.Learn(this);
            Powers.Add(power);
            UpdateAvailableCommands();
        }

        public void JoinGame(Game game, IPlayer player)
        {
            Game = game;
            Player = player;
        }

        internal void ResolveAttack(ConflictTarget target)
        {
//            if (target.IsWin)
//                Triggers.Send(HeroTrigger.FightWon);
            if (!(target.Conflict is Necromancer))
                LoseSecrecy("Attack");
//            DisplayConflictState();
        }

        public List<ITactic> GetAvailableFightTactics()
        {
            return GetAvailableTactics().Where(x => x.Type == TacticType.Fight).ToList();
        }

        public List<ITactic> GetAvailableTactics()
        {
            return _tactics.Values.Where(x => x.IsAvailable(this)).ToList();
        }

        public IEnumerable<Location> GetValidMovementLocations(bool onlyAdjacent = true)
        {
            var locations = onlyAdjacent ? Space.AdjacentLocations : Game.GetAllLocations();
            var blocks = _stash.GetAll<PreventMovementEffect>();
            var valid = locations.Where(loc => !blocks.Any(block => block.Matches(loc)));
            return valid;
        }

        public IPower GetPower(string name)
        {
            return Powers.SingleOrDefault(x => x.Name == name);
        }

        public void SelectTactic(string tacticName, ICollection<int> targetIds)
        {
            var targetCount = targetIds.Count;
            var targetsAreValid = ConflictState.AvailableTargets.Select(x => x.Id).Intersect(targetIds).Count() == targetCount
                                  && targetCount >= ConflictState.MinTarget && targetCount <= ConflictState.MaxTarget;
            if (!targetsAreValid)
                throw new Exception("Invalid targets.");

            var tacticInfo = ConflictState.AvailableTactics.SingleOrDefault(x => x.Name == tacticName);
            var tacticIsValid = tacticInfo != null;
            if (!tacticIsValid)
                throw new Exception($"Invalid tactic: {tacticName}");

            var tactic = _tactics[tacticName];
            var selectedTargets = from id in targetIds
                join target in ConflictState.AvailableTargets on id equals target.Id
                select new ConflictTarget(target.Conflict, target, tactic.Type);
            ConflictState.SelectedTargets = selectedTargets.ToList();
            ConflictState.SelectedTactic = tacticInfo;

            tactic.Use(this);

            CurrentRoll.ModifierType = tactic.GetRollType();
            CurrentRoll.TargetNumber = ConflictState.SelectedTargets.Min(x => x.TargetNumber);
            CurrentRoll.BaseDiceCount = tactic.GetDiceCount();
            CurrentRoll.BaseName = tactic.Name;

            CurrentRoll.Roll();
            ConflictState.Roll = CurrentRoll.AdjustedRoll;
            UpdateAvailableCommands();
            DisplayConflictState();
        }

        public void AcceptRoll()
        {
            var roll = CurrentRoll.Accept();
            Triggers.Send(HeroTrigger.RollAccepted, roll);
            if (CurrentRoll?.TargetNumber > 0)
                CurrentRoll = null;
        }

        public void AddTactic(ITactic tactic)
        {
            _tactics.Add(tactic.Name, tactic);
        }

        public void AddModifier(IModifier rollModifier)
        {
            _modifiers.Add(rollModifier);
        }

        public IEnumerable<IModifier> GetModifiers()
        {
            return _modifiers
                .Concat(Inventory.OfType<IModifier>())
                .Concat(Space.GetModifiers());
        }

        public void AssignDiceToTargets(ICollection<TargetDieAssignment> assignments)
        {
            var validRolls = assignments.Select(x => x.DieValue).Intersect(CurrentRoll.AdjustedRoll).Count() == assignments.Count;
            if (!validRolls)
                throw new Exception("Invalid die values specified.");
            var selectedTargets = ConflictState.SelectedTargets.ToList();
            var validTargets = assignments.Select(x => x.TargetId).Intersect(selectedTargets.Select(x => x.Id)).Count() == assignments.Count;
            if (!validTargets)
                throw new Exception("Invalid targets specified.");

            // Assign the die values
            var targetDieValues = (from target in selectedTargets
                join assignment in assignments on target.Id equals assignment.TargetId
                select new {target, assignment.DieValue}).ToList();
            targetDieValues.ForEach(x => x.target.ResultDie = x.DieValue);

            // resolve attacks
            foreach (var target in selectedTargets)
                ResolveAttack(target);
        }

        public void RemoveModifiers(string name)
        {
            _modifiers.RemoveAll(x => x.Name == name);
        }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command.Name, command);
        }

        public ICommand GetCommand(string commandName)
        {
            if (_commands.ContainsKey(commandName))
                return _commands[commandName];
            var space = Space;
            var itemAction = GetLocationInventory().OfType<ICommand>().FirstOrDefault(item => item.Name == commandName);
            if (itemAction != null)
                return itemAction;
            var locationAction = space.GetAction(commandName);
            if (locationAction != null)
                return locationAction;
            throw new CommandNotAvailableException(this, commandName);
        }

        public bool IsAffectedByBlight<T>() where T : IBlight
        {
            return GetActiveBlights<T>().Any() ;
        }

        public IEnumerable<T> GetActiveBlights<T>() where T : IBlight
        {
            return Space.GetActiveBlights<T>(this);
        }

        public bool HasCommand(string actionName)
        {
            return _commands.ContainsKey(actionName);
        }

        public void DisplayCurrentEvent()
        {
            Player.DisplayEvent(EventModel.From(CurrentEvent));
            Player.State = PlayerState.Event;
        }

        public void EndEvent()
        {
            CurrentEvent = null;
            Player.DisplayEvent(null);
            ContinueTurn();
        }

        internal void ContinueTurn()
        {
            if (State == HeroState.TurnStarted)
            {
                ContinueStartTurn();
                return;
            }
            State = HeroState.ContinuingTurn;
            // Face any undefeated enemies
            if (Enemies.Any())
            {
                DefendAgainst(Enemies);
                return;
            }
            // Handle any unresolved events
            if (EventQueue.Any())
            {
                var eventName = EventQueue.Dequeue();
                DoEvent(eventName);
                return;
            }

            if (_endingTurn)
                ContinueEndTurn();

            State = HeroState.TakingTurn;
            UpdateAvailableCommands();
            AutoEndTurn();
        }

        private void AutoEndTurn()
        {
            if (State != HeroState.TakingTurn) return;
            if (AvailableCommands.Count != 1) return;
            var endTurn = AvailableCommands.Single() as EndTurn;
            endTurn?.Execute(this);
        }

        public void RollEventDice(IRollHandler rollHandler)
        {
            var state = SetRoll(RollBuilder.Create(rollHandler).Type(ModifierType.EventDice).Base("Event", 1));
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
            var enemy = EnemyFactory.Create(enemyName);
            FaceEnemy(enemy);
        }

        public void FaceEnemy(IEnemy enemy)
        {
            Enemies.Add(enemy);
            DefendAgainst(Enemies);
        }

        public void SelectEventOption(string optionCode)
        {
            var validCodes = CurrentEvent.Options.Select(x=>x.Code).ToList();
            if (!validCodes.Contains(optionCode))
                throw new ArgumentOutOfRangeException(nameof(optionCode), optionCode, $"Valid codes: {validCodes.ToCsv()}");
            CurrentEvent.SelectedOption = optionCode;
            if (!Game.Triggers.Send(GameTrigger.EventOptionSelected))
                return;
            ResolveSelectedEventOption();
        }

        public void ResolveSelectedEventOption()
        {
            var card = EventFactory.CreateCard(CurrentEvent.Name);
            // An event can't be ignored after it has been resolved
            CurrentEvent.IsIgnorable = false;
            card.Resolve(this, CurrentEvent.SelectedOption);
        }

        public void RefreshPowers()
        {
            foreach (var power in Powers)
                power.Refresh();
        }

        public bool HasHolyRelic() => Inventory.OfType<HolyRelic>().Any();

        public bool IsTargetNecromancer()
        {
            var conflictState = ConflictState;
            if (conflictState?.SelectedTargets == null || conflictState.SelectedTargets.Count != 1) return false;
            return conflictState.SelectedTargets.First().Name == "Necromancer";
        }

        public void AddToInventory(IItem item)
        {
            item.SetOwner(this);
            Inventory.Add(item);
            UpdateAvailableCommands();
        }

        public IEnumerable<IItem> GetLocationInventory()
        {
            var heroesAtCurrentLocation = Game.Heroes.Where(hero => hero.Location == Location);
            return heroesAtCurrentLocation.SelectMany(hero => hero.GetInventory());
        }

        public void UpdateAvailableCommands()
        {
            AvailableCommands = GetAvailableCommands();
            Player.UpdateHeroCommands(new HeroActionModel(this));
        }

        public void DisplayConflictState()
        {
            UpdateAvailableCommands();
            if (ConflictState == null)
            {
                Player.DisplayConflict(null);
            }
            else
            {
                ConflictState.Roll = CurrentRoll?.AdjustedRoll;
                ConflictState.Display(Player);
            }
        }

        public void ResolveCurrentConflict()
        {
            var targets = ConflictState.SelectedTargets;
            if (targets.Any(x=>x.ResultDie == 0))
                throw new InvalidOperationException("All selected targets should have results assigned.");

            // Shrouds are priority target since they prevent destruction of other blight types
            var prioritizedTargets = targets.OrderBy(t => t.Conflict is Shroud ? 0 : 1);
            foreach (var target in prioritizedTargets)
            {
                if (target.TacticType == TacticType.Fight &&  target.IsWin)
                    Triggers.Send(HeroTrigger.FightWon);

                target.Resolve(this);
                var enemy = target.Conflict as IEnemy;
                if (enemy != null)
                    Enemies.Remove(enemy);
            }
            ConflictState = null;
            CurrentRoll = null;
            DisplayConflictState();
            ContinueTurn();
        }

        public bool CanSpendGrace => Grace > 0 || (Intercession?.CanIntercedeFor(this) ?? false);

        public bool CanSpendSecrecy => Secrecy > 0;

        internal Intercession Intercession { get; set; }

        public IEnumerable<IItem> GetInventory()
        {
            return Inventory;
        }

        public void ShufflePowerDeck()
        {
            _powerDeck = _powerDeck.Shuffle();
        }

        public void RemoveFromInventory(IItem item)
        {
            Inventory.Remove(item);
            UpdateAvailableCommands();
        }

        public void ExecuteCommand(string commandName)
        {
            var command = GetCommand(commandName);
            if (!command.IsAvailable(this))
                throw new CommandNotAvailableException(this, command);
            command.Execute(this);
            if (command is IAction)
                IsActionAvailable = false;
            UpdateAvailableCommands();
            AutoEndTurn();
        }

        public void AddFreeAction(IActionFilter actionFilter = null)
        {
            _hasFreeAction = true;
            _freeActionFilter = actionFilter;
            UpdateAvailableCommands();
        }

        public bool HasFreeAction => _hasFreeAction;
        internal void StartTurn()
        {
            Game.ActingHero = this;
            _movedDuringTurn = false;
            State = HeroState.TurnStarted;
            var commands = GetAvailableCommands().OfType<IStartOfTurnCommand>().Where(cmd => !(cmd is ContinueTurn));
            if (commands.Any())
            {
                Game.UpdateAvailableCommands();
                return;
            }

            Game.UpdateAvailableCommands();
            ContinueStartTurn();
        }

        private void ContinueStartTurn()
        {
            var necromancer = Game.Necromancer;
            var withNecromancer = Location == necromancer.Location;
            if (withNecromancer)
                LoseSecrecy("Necromancer");
            if (GetInventory().Any(item => item is HolyRelic))
                LoseSecrecy("Holy Relic");
            Triggers.Send(HeroTrigger.StartedTurn);

            if (withNecromancer && Secrecy == 0)
            {
                FaceEnemy(necromancer);
            }
            else if (Location != Location.Monastery)
                DrawEvent();
            else
            {
                State = HeroState.TakingTurn;
                UpdateAvailableCommands();
            }
        }

        internal void EndTurn()
        {
            _endingTurn = true;
            IsActionAvailable = false;

            var spies = GetActiveBlights<Spies>().Count();
            LoseSecrecy(spies, "Spies");

            // Enemy lair blights generate enemies to fight/elude
            var lairs = GetActiveBlights<EnemyLair>();
            var enemies = lairs.GenerateEnemies().ToList();
            if (enemies.Any())
            {
                Enemies.AddRange(enemies);
                DefendAgainst(Enemies);
                return;
            }

            ContinueEndTurn();
        }

        private void ContinueEndTurn()
        {
            _endingTurn = false;
            if (Location == Location.Monastery && !_movedDuringTurn)
                GainSecrecy(1, DefaultSecrecy);
            Game.ActingHero = null;
            HasTakenTurn = true;
            Triggers.Send(HeroTrigger.TurnEnded);
            Game.Triggers.Send(GameTrigger.HeroTurnEnded, this);
            if (Game.Heroes.All(x => x.HasTakenTurn))
                Game.Necromancer.StartTurn();
            else
                Game.UpdateAvailableCommands();
        }

        public void SetLocation(Location location)
        {
            Location = location;
            UpdateAvailableCommands();
            UpdateHeroStatus();
        }

        private void DefendAgainst(List<IEnemy> enemies)
        {
            State = HeroState.FacingEnemy;
            SetRoll(RollBuilder.Create<DefenseRollHandler>());
            var validTacticTypes = new List<TacticType>();
            if (enemies.Any(x=>x.Fight.HasValue))
                validTacticTypes.Add(TacticType.Fight);
            if (enemies.Any(x=>x.Elude.HasValue))
                validTacticTypes.Add(TacticType.Elude);
            var availableTactics = GetAvailableTactics().Where(t => validTacticTypes.Contains(t.Type));
            ConflictState = new ConflictState
            {
                MaxTarget = 1,
                MinTarget = 1,
                AvailableTargets = enemies.GetTargetInfo(),
                AvailableTactics = availableTactics.GetInfo(this)
            };
            DisplayConflictState();
        }

        internal void StartNewDay()
        {
            IsActionAvailable = true;
            HasTakenTurn = LoseNextTurn;
            LoseNextTurn = false;
            UpdateAvailableCommands();
        }

        public void AddRollModifier(IRollModifier modifier)
        {
            _rollModifiers.Add(modifier);
        }

        public IEnumerable<IRollModifier> GetRollModifiers()
        {
            var itemModifiers = GetLocationInventory().OfType<IRollModifier>();
            return _rollModifiers.Concat(itemModifiers);
        }

        public void TradeItemTo(Hero toHero, int itemId)
        {
            if (toHero.Location != Location) return;
            var item = GetItem(itemId);
            if (item is HolyRelic && toHero.Inventory.Any(x => x is HolyRelic)) return;
            RemoveFromInventory(item);
            toHero.AddToInventory(item);
            if (item is HolyRelic)
                toHero.LoseSecrecy("Holy Relic");
        }

        internal void DrawSearchResults(int count)
        {
            var results = Game.DrawSearchResult(Location, count);
            DisplaySearch(new SearchResultSelectedHandler(), results);
        }

        public void DisplaySearch(ICallbackHandler<Find> callbackHandler, IEnumerable<Find> finds = null)
        {
            Player.DisplaySearch(SearchModel.Create(this, finds), Callback.For(this, callbackHandler));
        }

        public Task<Location> SelectLocation(List<string> choices)
        {
            State = HeroState.SelectingLocation;
            return Player.SelectLocation(choices);
        }

        public async Task<IEnumerable<int>> SelectBlights(BlightSelectionModel selection)
        {
            State = HeroState.SelectingBlights;
            return await Player.SelectBlights(selection);
        }

        public HeroData GetData()
        {
            return new HeroData
            {
                Name = Name,
                Grace = Grace,
                Secrecy = Secrecy,
                Location = Location,
                PowerDeck = PowerDeck,
                Powers = Powers.Select(PowerData.Create).ToList(),
                Inventory = Inventory.Select(x => x.Name).ToList(),
            };
        }

        public async Task<Hero> SelectHero(IEnumerable<Hero> validTargets)
        {
            var model = new HeroSelectionModel(validTargets);
            State = HeroState.SelectingHero;
            var selectedHero = await Player.SelectHero(model);
            return selectedHero;
        }

        public void RemoveCommand(ICommand command)
        {
            _commands.Remove(command.Name);
            UpdateAvailableCommands();
        }
    }
}