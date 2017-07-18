using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Inspire : ActivateablePower, ITargetable
    {
        private readonly DeactivateInspireCommand _deactivateCommand;
        private Hero _target;

        public Inspire()
        {
            Name = "Inspire";
            StartingPower = true;
            Text = "Activate on a hero in your location.";
            ActiveText = "Deactivate before any die roll for +3d.";
            _deactivateCommand = new DeactivateInspireCommand(this);
        }

        public void SetTarget(string targetName)
        {
            _target = Owner.Game.GetHero(targetName);
        }

        public string GetTarget()
        {
            return _target?.Name;
        }

        public override async void Activate(Hero hero)
        {
            base.Activate(hero);
            if (_target == null)
            {
                var validHeroes = hero.Game.Heroes.Where(h => h.Location == hero.Location);
                _target = await hero.SelectHero(validHeroes);
            }
            _target.AddCommand(_deactivateCommand);
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero))
                return false;
            _target.RemoveCommand(_deactivateCommand);
            _target.AddModifier(StaticRollBonus.AnyRoll(Name, 3));
            _target.Triggers.Add(HeroTrigger.RollAccepted, Name, new RemoveInspireBonus() );
            _target = null;
            return true;
        }

        internal class RemoveInspireBonus : ITriggerHandler<Hero>
        {
            public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
            {
                hero.RemoveModifiers(source);
                hero.Triggers.RemoveBySource(source);
                return Task.CompletedTask;
            }
        }

        internal class DeactivateInspireCommand : ICommand
        {
            private readonly Inspire _inspire;

            public DeactivateInspireCommand(Inspire inspire)
            {
                _inspire = inspire;
            }

            public string Name => "Deactivate Inspire";
            public string Text => _inspire.ActiveText;
            public bool RequiresAction => false;

            public bool IsAvailable(Hero hero)
            {
                return _inspire._target.IsTakingTurn && !_inspire.IsExhausted;
            }

            public void Execute(Hero hero)
            {
                _inspire.Deactivate(_inspire.Owner);
            }
        }
    }
}