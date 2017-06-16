using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes.Impl;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public abstract class Hero : ITriggerRegistrar
    {
        private readonly List<IPower> _powerDeck;
        private Stash _stash;

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
            throw new System.NotImplementedException();
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

        public void AttackBlight()
        {
            var space = GetSpace();
            var blight = space.Blights.Single();
            var selectedTactic = SelectTactic();
            var result = selectedTactic.GetResult();
            ResolveAttack(blight, result);
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

        internal Tactic SelectTactic()
        {
            var tactics = Powers.Where(x => x.Type == PowerType.Tactic && x.IsUsable()).Cast<Tactic>().ToList();
            var fightTactics = tactics
                .Where(x => (x.TacticType & TacticType.Fight) > 0)
                .ToList();
            var availableTactics = new[] {Tactic.None(this)}.Concat(fightTactics).ToList();
            var selectedTactic = availableTactics.Count == 1 ? availableTactics.Single() : Player.ChooseTactic(availableTactics);
            return selectedTactic;
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

        private IPower GetPower(string name)
        {
            return Powers.SingleOrDefault(x=>x.Name==name);
        }

        public void RemoveBySource<T>(string name)
        {
            _stash.RemoveBySource<T>(name);
        }
    }

}
