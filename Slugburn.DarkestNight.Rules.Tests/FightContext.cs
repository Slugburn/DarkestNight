using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class FightContext
    {
        private readonly FakePlayer _player;
        private string _action;
        private Blight[] _targets;
        private string _tactic;

        public FightContext(FakePlayer player, Hero hero)
        {
            _player = player;
            _action = "Attack";
            _tactic = "None";
            var blights = hero.GetSpace().Blights;
            if (blights.Count == 1)
                _targets = new[] {blights.First().Type};
        }

        public Action<PlayerActionContext> PlayerActions { get; private set; }

        public string GetAction() => _action;
        public string GetTactic() => _tactic;
        public Blight[] GetTargets() => _targets;

        public FightContext Action(string action)
        {
            _action = action;
            return this;
        }

        public FightContext Tactic(string tactic)
        {
            _tactic = tactic;
            return this;
        }

        public FightContext Target(params Blight[] targets)
        {
            _targets = targets;
            return this;
        }

        public FightContext Roll(params int[] roll)
        {
            _player.AddUpcomingRolls(roll);
            return this;
        }
    }
}