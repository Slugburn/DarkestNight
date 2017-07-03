using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Intercession : BonusPower, ICallbackHandler
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

        public bool IntercedeForLostGrace(Hero other, int amount)
        {
            if (!Owner.CanSpendGrace) return false;
            if (Owner.Grace < amount) return false;
            var question = new QuestionModel("Intercession",
                $"Should {Owner.Name} spend {amount} Grace instead of {other.Name} losing {amount} Grace?", new[] {"Yes", "No"});
            other.Player.DisplayAskQuestion(question, Callback.ForPower(Owner, this, $"{other.Name}:loss:{amount}"));
            return true;
        }

        public bool IntercedeForSpentGrace(Hero other, int amount)
        {
            if (!CanIntercedeFor(other)) return false;
            if (Owner.Grace < amount) return false;
            if (other.Grace < amount)
            {
                Owner.SpendGrace(amount);
                return true;
            }
            var question = new QuestionModel("Intercession",
                $"Should {Owner.Name} spend {amount} Grace instead of {other.Name} spending {amount} Grace?", new [] {"Yes", "No"});
            other.Player.DisplayAskQuestion(question, Callback.ForPower(Owner, this, $"{other.Name}:spent:{amount}"));
            return true;
        }


        public void HandleCallback(Hero hero, string path, object data)
        {
            var args=path.Split(':');
            var otherName = args[0];
            var op = args[1];
            var amount = int.Parse(args[2]);
            var answer = (string) data;
            var other = hero.Game.GetHero(otherName);
            if (op == "loss")
            {
                if (answer == "Yes")
                    Owner.SpendGrace(amount);
                else
                    other.LoseGrace(amount, false);
            }
            else if (op == "spent")
            {
                if (answer == "Yes")
                    Owner.SpendGrace(amount);
                else
                    other.SpendGrace(amount, false);
            }
        }

    }
}