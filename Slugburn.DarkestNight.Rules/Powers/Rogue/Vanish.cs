using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Vanish : TacticPower, ITriggerHandler<Hero>
    {
        public Vanish()
        {
            Name = "Vanish";
            StartingPower = true;
            Text = "Elude with 2 dice. Gain 1 Secrecy (up to 7) if you roll 2 successes.";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddTactic(new PowerTactic(this, TacticType.Elude, 2));
            Owner.Triggers.Add(HeroTrigger.RollAccepted, Name, this);
        }

        public Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
        {
            var conflict = hero.ConflictState;
            if (conflict == null || conflict.SelectedTactic.Name != Name) return Task.CompletedTask;
            var roll = context.GetState<RollState>();
            if (roll.Successes > 1)
                hero.GainSecrecy(1, 7);
            return Task.CompletedTask;
        }
    }
}