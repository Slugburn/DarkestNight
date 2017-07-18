using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Stealth : BonusPower, ITriggerHandler<Hero>
    {
        public Stealth()
        {
            Name = "Stealth";
            Text = "Any time you lose or spend Secrecy, you can spend 1 Grace instead.";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.OverrideCanSpendSecrecy(hero => hero.Secrecy > 0 || !IsExhausted && hero.CanSpendGrace);
            Owner.Triggers.Add(HeroTrigger.LosingSecrecy, Name, this);
        }

        public async Task HandleTriggerAsync(Hero hero, string source, TriggerContext context)
        {
            var answer = await hero.Player.AskQuestion(new QuestionModel("Stealth", "Spend 1 Grace instead of Secrecy?", new[] {"Yes", "No"}));
            if (answer != "Yes") return;
            hero.SpendGrace(1);
            context.Cancel = true;
        }
    }
}