using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class TacticContext : IFakeRollContext
    {
        public TacticContext(Hero hero, string defaultTactic)
        {
            _tactic = defaultTactic;
            var blights = hero.GetBlights();
            if (blights.Count != 1) return;
            var blight = blights.First();
            var target = blight.IsEnemyGenerator()
                ? ((EnemyGenerator) blight.GetDetail()).EnemyName
                : blight.ToString();
            _targets = new[] {target}.ToList();
        }

        private string _tactic;
        private List<string> _targets;

        public string GetTactic() => _tactic;

        public ICollection<string> GetTargets() => _targets;

        public TacticContext Tactic(string tactic)
        {
            _tactic = tactic;
            return this;
        }
    }
}