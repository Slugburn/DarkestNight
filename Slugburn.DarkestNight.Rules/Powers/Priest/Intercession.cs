using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class Intercession : Bonus, ICallbackHandler
    {
        public Intercession()
        {
            Name = "Intercession";
            StartingPower = true;
            Text = "Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            foreach (var other in hero.Game.Heroes.Where(h=>h!=hero))
            {
                other.Intercession = this;
//                other.Triggers.Add(HeroTrigger.CreatingEventOptions, Name, new IntercessionEventOptionCreation(hero, this));
//                other.Triggers.Add(HeroTrigger.EventOptionSelected, Name, new IntercessionEventOptionSelected(hero, this) );
            }
        }

        internal class IntercessionEventOptionCreation : ITriggerHandler<Hero>
        {
            private readonly Hero _owner;
            private readonly Intercession _power;

            public IntercessionEventOptionCreation(Hero owner, Intercession power)
            {
                _owner = owner;
                _power = power;
            }

            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                if (!_owner.CanSpendGrace) return;
                if (!_power.IsUsable(_owner)) return;
                var options = context.GetState<List<EventOption>>();
                if (options.Any(x => x.Code == "spend-grace"))
                    options.Insert(0, new EventOption("spend-grace-intercession", "Spend Grace [Intercession]", null));
            }
        }

        public bool CanIntercedeFor(Hero other)
        {
            if (!_hero.CanSpendGrace) return false;
            if (!IsUsable(_hero)) return false;
            return _hero.Location == other.Location;
        }

        public bool IntercedeForLostGrace(Hero other, int amount)
        {
            if (!_hero.CanSpendGrace) return false;
            if (_hero.Grace < amount) return false;
            var question = new PlayerAskQuestion("Intercession",
                $"Should {_hero.Name} spend {amount} Grace instead of {other.Name} losing {amount} Grace?");
            other.Player.DisplayAskQuestion(question, Callback.ForPower(_hero, this, $"{other.Name}:loss:{amount}"));
            return true;
        }

        public bool IntercedeForSpentGrace(Hero other, int amount)
        {
            if (!CanIntercedeFor(other)) return false;
            if (_hero.Grace < amount) return false;
            if (other.Grace < amount)
            {
                _hero.SpendGrace(amount);
                return true;
            }
            var question = new PlayerAskQuestion("Intercession",
                $"Should {_hero.Name} spend {amount} Grace instead of {other.Name} spending {amount} Grace?");
            other.Player.DisplayAskQuestion(question, Callback.ForPower(_hero, this, $"{other.Name}:spent:{amount}"));
            return true;
        }


        internal class IntercessionEventOptionSelected : ITriggerHandler<Hero>
        {
            private readonly Hero _owner;

            public IntercessionEventOptionSelected(Hero owner, Intercession power)
            {
                _owner = owner;
            }

            public void HandleTrigger(Hero hero, string source, TriggerContext context)
            {
                _owner.SpendGrace(1);
                hero.Grace += 1;
                hero.CurrentEvent.SelectedOption = "spend-grace";
            }
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            var args=path.Split(':');
            var otherName = args[0];
            var op = args[1];
            var amount = int.Parse(args[2]);
            var answer = (bool) data;
            var other = hero.Game.GetHero(otherName);
            if (op == "loss")
            {
                if (answer)
                    _hero.SpendGrace(amount);
                else
                    other.LoseGrace(amount, false);
            }
            else if (op == "spent")
            {
                if (answer)
                    _hero.SpendGrace(amount);
                else
                    other.SpendGrace(amount, false);
            }
        }

    }
}