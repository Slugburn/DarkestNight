using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Diversion : ActionPower, ITargetable
    {
        public Diversion()
        {
            Name = "Diversion";
            Text = "Spend 1 Secrecy to negate the effects of one blight in your location until the Necromancer ends a turn there.";
        }

        public List<IBlight> Targets { get; } = new List<IBlight>();

        public void SetTarget(string targetName)
        {
            var targetIds = targetName.Split(',').Select(int.Parse);
            foreach (var targetId in targetIds)
            {
                var blight = Owner.Game.GetBlight(targetId);
                UseOn(blight);
            }
        }

        public string GetTarget()
        {
            return Targets.Select(x => x.Id.ToString()).ToCsv();
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero)
                   && hero.CanSpendSecrecy
                   && hero.GetBlights().Any();
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new DiversionCommand(this));
        }

        private void UseOn(IBlight blight)
        {
            var supression = new DiversionBlightSupression(this, blight);
            Owner.Game.AddBlightSupression(supression);
            Owner.Game.Triggers.Add(GameTrigger.NecromancerTurnEnded, supression.Name, new DiversionOnNecromancerTurnEnded(supression));
            Owner.Game.Triggers.Add(GameTrigger.BlightDestroyed, supression.Name, new DiversionOnBlightDestroyed(this, blight.Id));
        }

        private class DiversionCommand : PowerAction
        {
            public DiversionCommand(IActionPower power) : base(power) { }

            public override async void Execute(Hero hero)
            {
                var validBlights = hero.Space.Blights;
                var blightIds = await hero.Player.SelectBlights(BlightSelectionModel.Create(Name, validBlights, 1));
                var blightId = blightIds.Single();
                var blight = hero.Game.GetBlight(blightId);
                var power = (Diversion) Power;
                power.Targets.Add(blight);
                power.UseOn(blight);
            }
        }

        private class DiversionBlightSupression : IBlightSupression
        {
            public DiversionBlightSupression(Diversion power, IBlight blight)
            {
                Power = power;
                Blight = blight;
            }

            public Diversion Power { get; }
            public IBlight Blight { get; }

            public string Name => $"{Power.Name}:{Blight.Id}";

            public bool IsSupressed(IBlight blight, Hero hero = null)
            {
                return blight == Blight;
            }
        }

        private class DiversionOnNecromancerTurnEnded : ITriggerHandler<Game>
        {
            private readonly DiversionBlightSupression _supression;

            public DiversionOnNecromancerTurnEnded(DiversionBlightSupression supression)
            {
                _supression = supression;
            }

            public void HandleTrigger(Game game, string source, TriggerContext context)
            {
                if (game.Necromancer.Location != _supression.Blight.Location) return;

                _supression.Power.Targets.Remove(_supression.Blight);
                game.RemoveBlightSupression(_supression.Name);
                game.Triggers.RemoveBySource(_supression.Name);
            }
        }

        private class DiversionOnBlightDestroyed : ITriggerHandler<Game>
        {
            private readonly int _blightId;
            private readonly Diversion _power;

            public DiversionOnBlightDestroyed(Diversion power, int blightId)
            {
                _power = power;
                _blightId = blightId;
            }

            public void HandleTrigger(Game game, string source, TriggerContext context)
            {
                _power.Targets.RemoveAll(x => x.Id == _blightId);
            }
        }
    }
}