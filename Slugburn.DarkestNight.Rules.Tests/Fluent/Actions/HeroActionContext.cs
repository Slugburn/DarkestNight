using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class HeroActionContext : When, IHeroActionContext
    {
        private readonly Hero _hero;

        public HeroActionContext(Game game, FakePlayer player) :base (game, player)
        {
            _hero = game.ActingHero;
        }


        public IHeroActionContext DrawsEvent(string eventName = null)
        {
            if (eventName != null)
            {
                // move event to first
                _hero.Game.Events.Remove(eventName);
                _hero.Game.Events.Insert(0, eventName);
            }
            if (_hero.Location == Location.Monastery)
                _hero.Location = Location.Village;
            _hero.DrawEvent();
            return this;
        }

        public IHeroActionContext RefreshesPower(string powerName)
        {
            var power = _hero.GetPower(powerName);
            power.Refresh();
            return this;
        }

        public IHeroActionContext MovesTo(string location)
        {
            _hero.MoveTo(location.ToEnum<Location>());
            return this;
        }

        public IHeroActionContext StartsTurn()
        {
            _hero.IsActionAvailable = true;
            _hero.StartTurn();
            return this;
        }

        public IHeroActionContext Fights(Action<TacticContext> actions)
        {
            FacesEnemies();
            SelectsTactic(actions, "Fight");
            Assert.That(_hero.ConflictState.SelectedTactic.Type, Is.EqualTo(TacticType.Fight));
            _hero.AcceptRoll();
            return this;
        }

        public IHeroActionContext Eludes(Action<TacticContext> actions)
        {
            FacesEnemies();
            SelectsTactic(actions, "Elude");
            Assert.That(_hero.ConflictState.SelectedTactic.Type, Is.EqualTo(TacticType.Elude));
            _hero.AcceptRoll();
            return this;
        }

        public IHeroActionContext SelectsTactic(Action<TacticContext> define = null, string defaultTactic = "Fight")
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