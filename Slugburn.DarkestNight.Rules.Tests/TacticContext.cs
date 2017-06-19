using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class TacticContext
    {
        private readonly FakeDie _die;

        public TacticContext(Hero hero, FakeDie die)
        {
            _die = die;
            _tactic = "Fight";
            var blights = hero.GetBlights();
            if (blights.Count == 1)
            {
                var blight = blights.First();
                var target = blight.IsEnemyGenerator()
                    ? ((EnemyGenerator) blight.GetDetail()).EnemyName
                    : blight.ToString();
                _targets = new[] {target}.ToList();
            }
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

        public TacticContext Rolls(params int[] roll)
        {
            _die.AddUpcomingRolls(roll);
            return this;
        }
    }
}