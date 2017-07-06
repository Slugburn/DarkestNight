using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

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

        public bool IntercedeForLostGrace(Hero other, int amount)
        {
            if (Owner.Location != other.Location) return false;
            if (!Owner.CanSpendGrace) return false;
            if (Owner.Grace < amount) return false;
            var question = new QuestionModel("Intercession",
                $"Should {Owner.Name} spend {amount} Grace instead of {other.Name} losing {amount} Grace?", new[] {"Yes", "No"});
            other.Player.DisplayAskQuestion(question, Callback.For(other, new IntercessionCallbackHandler(Owner, "loss", amount)));
            return true;
        }

        public bool IntercedeForSpentGrace(Hero other, int amount)
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
            other.Player.DisplayAskQuestion(question, Callback.For(other, new IntercessionCallbackHandler(Owner, "spent", amount)));
            return true;
        }


        private class IntercessionCallbackHandler : ICallbackHandler
        {
            private readonly Hero _owner;
            private readonly string _op;
            private readonly int _amount;

            public IntercessionCallbackHandler(Hero owner, string op, int amount)
            {
                _owner = owner;
                _op = op;
                _amount = amount;
            }

            public void HandleCallback(Hero hero, object data)
            {
                var other = hero;
                var answer = (string)data;
                switch (_op)
                {
                    case "loss":
                        if (answer == "Yes")
                            _owner.SpendGrace(_amount);
                        else
                            other.LoseGrace(_amount, false);
                        break;
                    case "spent":
                        if (answer == "Yes")
                            _owner.SpendGrace(_amount);
                        else
                            other.SpendGrace(_amount, false);
                        break;
                }
            }
        }
    }
}