using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class HeroActionContext
    {
        private readonly Hero _hero;

        public HeroActionContext(Hero hero)
        {
            _hero = hero;
        }


        public HeroActionContext DrawsEvent(string eventName=null)
        {
            if (eventName != null)
                _hero.Game.Events.Insert(0, eventName);
            if (_hero.Location == Location.Monastery)
                _hero.Location = Location.Village;
            _hero.DrawEvent();
            return this;
        }

        public HeroActionContext RefreshesPower(string powerName)
        {
            var power = _hero.GetPower(powerName);
            power.Refresh();
            return this;
        }

        public HeroActionContext MovesTo(Location location)
        {
            _hero.MoveTo(location);
            return this;
        }

        public HeroActionContext StartsTurn()
        {
            _hero.IsActionAvailable = true;
            _hero.StartTurn();
            return this;
        }

        public HeroActionContext Fights(Action<TacticContext> actions)
        {
            FacesEnemies();
            SelectsTactic(actions, "Fight");
            Assert.That(_hero.ConflictState.SelectedTactic.Type, Is.EqualTo(TacticType.Fight));
            _hero.AcceptRoll();
            return this;
        }

        public HeroActionContext Eludes(Action<TacticContext> actions)
        {
            FacesEnemies();
            SelectsTactic(actions, "Elude");
            Assert.That(_hero.ConflictState.SelectedTactic.Type, Is.EqualTo(TacticType.Elude));
            _hero.AcceptRoll();
            return this;
        }

        public HeroActionContext SelectsTactic(Action<TacticContext> define = null, string defaultTactic = "Fight")
        {
            var context = new TacticContext(_hero, defaultTactic);
            define?.Invoke(context);
            var blights = context.GetTargets();
            var targetIds = GetTargetIds(blights).ToList();
            _hero.SelectTactic(context.GetTactic(), targetIds);
            return this;
        }

        private IEnumerable<int> GetTargetIds(ICollection<string> targets)
        {
            var source = _hero.ConflictState.AvailableTargets.ToList();
            Assert.That(source.Select(x => x.Name).Intersect(targets), Is.EquivalentTo(targets), "Requested target is not valid.");
            foreach (var match in targets.Select(target => source.First(x => x.Name == target)))
            {
                source.Remove(match);
                yield return match.Id;
            }
        }

        private HeroActionContext FacesEnemies()
        {
            var enemies = _hero.GetBlights().GenerateEnemies();
            _hero.Enemies = enemies;
            new Defend().Act(_hero);
            return this;
        }
    }
}