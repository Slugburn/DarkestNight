using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

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
            hero.AddCommand(_deactivateCommand);
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero))
                return false;
            hero.RemoveCommand(_deactivateCommand);
            _target.CurrentRoll.AddDice(3);
            return true;
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
                return _inspire._target.CurrentRoll != null;
            }

            public void Execute(Hero hero)
            {
                _inspire.Deactivate(_inspire.Owner);
            }
        }
    }
}