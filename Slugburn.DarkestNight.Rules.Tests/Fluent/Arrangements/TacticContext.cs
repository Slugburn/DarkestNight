using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements
{
    public class TacticContext : IFakeContext
    {
        private readonly List<string> _targets;
        private string _tactic;

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

        public string GetTactic() => _tactic;

        public ICollection<string> GetTargets() => _targets;

        public TacticContext Tactic(string tactic)
        {
            _tactic = tactic;
            return this;
        }
    }
}