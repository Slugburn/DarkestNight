using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Intercession : BonusPower
    {
        public Intercession()
        {
            Name = "Intercession";
            StartingPower = true;
            Text = "Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.";
        }

        protected override void OnLearn()
        {
            foreach (var other in Owner.Game.Heroes.Where(h=>h!=Owner))
            {
                other.Intercession = this;
            }
        }

        public bool CanIntercedeFor(Hero other)
        {
            if (!Owner.CanSpendGrace) return false;
            if (!IsUsable(Owner)) return false;
            return Owner.Location == other.Location;
        }

        public async Task<bool> IntercedeForLostGrace(Hero other, int amount)
        {
            if (Owner.Location != other.Location) return false;
            if (!Owner.CanSpendGrace) return false;
            if (Owner.Grace < amount) return false;
            var question = new QuestionModel("Intercession",
                $"Should {Owner.Name} spend {amount} Grace instead of {other.Name} losing {amount} Grace?", new[] {"Yes", "No"});
            var answer = await other.Player.AskQuestion(question);
            if (answer == "Yes")
                Owner.SpendGrace(amount);
            else
                other.LoseGrace(amount, false);
            return true;
        }

        public async Task<bool> IntercedeForSpentGrace(Hero other, int amount)
        {
            if (Owner.Location != other.Location) return false;
            if (!CanIntercedeFor(other)) return false;
            if (Owner.Grace < amount) return false;
            if (other.Grace < amount)
            {
                Owner.SpendGrace(amount);
                return true;
            }
            var question = new QuestionModel("Intercession",
                $"Should {Owner.Name} spend {amount} Grace instead of {other.Name} spending {amount} Grace?", new [] {"Yes", "No"});
            var answer = await other.Player.AskQuestion(question);
            if (answer == "Yes")
                Owner.SpendGrace(amount);
            else
                other.SpendGrace(amount, false);
            return true;
        }
    }
}