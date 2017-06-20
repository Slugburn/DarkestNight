using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class FightContext : IFakeRollContext
    {
        private readonly FakePlayer _player;
        private string _action;
        private string[] _targets;
        private string _tactic;

        public FightContext(FakePlayer player, Hero hero)
        {
            _player = player;
            _action = "Attack";
            _tactic = "Fight";
            var blights = hero.GetSpace().Blights;
            if (blights.Count == 1)
                _targets = new[] {blights.First().ToString()};
        }

        public string GetAction() => _action;
        public string GetTactic() => _tactic;
        public string[] GetTargets() => _targets;

        public FightContext Action(string action)
        {
            _action = action;
            if (action == FightNecromancer.ActionName)
                _targets = new[] {"Necromancer"};
            return this;
        }

        public FightContext Tactic(string tactic)
        {
            _tactic = tactic;
            return this;
        }

        public FightContext Target(params string[] targets)
        {
            _targets = targets;
            return this;
        }
        public FightContext Target(params Blight[] targets)
        {
            _targets = targets.Select(x=>x.ToString()).ToArray();
            return this;
        }

        public FightContext UsePower(string powerName)
        {
            _player.SetUsePowerResponse(powerName, true);
            return this;
        }
    }
}