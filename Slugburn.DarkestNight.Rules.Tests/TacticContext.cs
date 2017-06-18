using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class TacticContext
    {
        private readonly FakePlayer _player;

        public TacticContext(FakePlayer player, Hero hero)
        {
            _player = player;
            _tactic = "Fight";
            if (hero.GetBlights().Count == 1)
                _targets = hero.GetBlights().Select(x=>x.Type).ToList();
        }

        private string _tactic;
        private List<Blight> _targets;

        public string GetTactic() => _tactic;

        public ICollection<Blight> GetTargets() => _targets;

        public TacticContext Tactic(string tactic)
        {
            _tactic = tactic;
            return this;
        }

        public TacticContext Rolls(params int[] roll)
        {
            _player.AddUpcomingRolls(roll);
            return this;
        }
    }
}