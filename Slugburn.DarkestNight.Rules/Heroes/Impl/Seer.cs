using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Seer : Hero
    {
        public Seer()
            : base(
                "Seer", 4, 6, new Destiny(), new Dowse(), new Foreknowledge(), new Hope(), new Prediction(), new Premonition(), new ProphecyOfDoom(),
                new ProphecyOfFortune(), new ProphecyOfSafety(), new ProphecyOfSanctuary())
        {
        }

        class Destiny : Bonus
        {
            public Destiny()
            {
                Name = "Destiny";
                Text = "+1 die in fights.";
            }
        }

        class Dowse : ActionPower
        {
            public Dowse()
            {
                Name = "Dowse";
                StartingPower = true;
                Text = "Exhaust to draw one search result for your location without rolling dice.";
            }

            protected override bool TakeAction()
            {
                throw new System.NotImplementedException();
            }
        }

        class Foreknowledge : Bonus
        {
            public Foreknowledge()
            {
                Name = "Foreknowledge";
                Text = "When you experience an event, draw an extra card and discard 1.";
            }
        }

        class Hope : ActionPower
        {
            public Hope()
            {
                Name = "Hope";
                Text = "Exhaust and spend 1 Grace to cause -1 Darkness.";
            }

            protected override bool TakeAction()
            {
                throw new System.NotImplementedException();
            }
        }

        class Prediction : ActionPower
        {
            public Prediction()
            {
                Name = "Prediction";
                StartingPower = true;
                Text = "Roll 2 dice and add them to this card. You may use all dice on this card instead of making any roll. When you do, clear this card.";
            }

            protected override bool TakeAction()
            {
                throw new System.NotImplementedException();
            }
        }

        class Premonition : TacticPower
        {
            public Premonition() : base(TacticType.Elude)
            {
                Name = "Premonition";
                StartingPower = true;
                Text = "Elude with 3 dice. Exhaust if you roll fewer than 2 successes.";
            }
        }

        class ProphecyOfDoom : Bonus
        {
            public ProphecyOfDoom()
            {
                Name = "Prophecy of Doom";
                Text = "Exhaust at the start of any hero's turn. Roll 3 dice and choose 1; use that as the Necromancer's next movement roll.";
            }
        }

        class ProphecyOfFortune : Bonus
        {
            public ProphecyOfFortune()
            {
                Name = "Prophecy of Fortune";
                StartingPower = true;
                Text = "Exhaust at the start of any hero's turn. That hero gains +1 die on all rolls this turn.";
            }
        }

        public class ProphecyOfSafety : Bonus
        {
            public ProphecyOfSafety()
            {
                Name = "Prophecy of Safety";
                Text = "Exhaust at the start of any hero's turn. That hero need not lose or spend Grace this turn.";
            }
        }

        public class ProphecyOfSanctuary : Bonus
        {
            public ProphecyOfSanctuary()
            {
                Name = "Prophecy of Santuary";
                Text = "Exhaust at the start of any hero's turn. That hero gains 1 Secrecy (up to 7) and need not lose or spend Secrecy this turn.";
            }
        }
    }
}