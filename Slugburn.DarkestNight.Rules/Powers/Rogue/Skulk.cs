using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Skulk : TacticPower
    {
        private readonly PowerTactic _tactic;

        public Skulk()
        {
            Name = "Skulk";
            Text = "Elude with 2 dice and add 1 to the highest die.";
            _tactic = new PowerTactic(this, TacticType.Elude, 2);
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddTactic(_tactic);
            Owner.AddRollModifier(new SkulkRollModifer(_tactic));
        }

        private class SkulkRollModifer : IRollModifier
        {
            private readonly PowerTactic _tactic;

            public SkulkRollModifer(PowerTactic tactic)
            {
                _tactic = tactic;
            }

            public ICollection<int> Modify(Hero hero, ModifierType modifierType, ICollection<int> roll)
            {
                if (modifierType != ModifierType.EludeDice) return roll;
                if (hero.ConflictState.SelectedTactic.Name != _tactic.Name) return roll;
                return roll.AddOneToHighest();
            }
        }
    }
}