using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Wizard : Hero
    {

        class Teleport : ActionPower
        {
            public Teleport()
            {
                Name = "Teleport";
                StartingPower = true;
                Text = "Exhaust to move directly to any location, gaining 2 Secrecy (up to 5).";
            }
        }

        class LightningStrike: TacticPower
        {
            public LightningStrike() : base(TacticType.Fight)
            {
                Name = "Lightning Strike";
                StartingPower = true;
                Text = "Fight with 3 dice. Exhaust if you succeed.";
            }
        }

        class RuneOfNullification : ActionPower
        {
            public RuneOfNullification()
            {
                Name = "Rune of Nullification";
                StartingPower = true;
                Text = "Deactivate all Runes. Activate and choose a type of blight.";
                ActiveText = "That type of blight has no effect.";
            }
        }

        class Invisibility : TacticPower
        {
            public Invisibility() : base(TacticType.Elude)
            {
                Name = "Invisibility";
                StartingPower = true;
                Text = "Activate.";
                ActiveText = "+2 dice when eluding. Deactivate and exhaust if you fail a combat or use another Tactic.";
            }
        }

        class Fiendfire : TacticPower
        {
            public Fiendfire() : base(TacticType.Fight)
            {
                Name = "Fiendfire";
                Text = "Exhaust to fight with 5 dice.";
            }
        }

        class ArcaneEnergy : Bonus
        {
            public ArcaneEnergy()
            {
                Name = "Arcane Energy";
                Text = "Exhaust at any time to refresh all you other powers.";
            }
        }

        class RuneOfInterference : ActionPower
        {
            public RuneOfInterference()
            {
                Name = "Rune of Interference";
                Text = "Deactivate all Runes. Activate.";
                ActiveText = "Roll 1 die when a blight is created. If you roll a 6, destroy it.";
            }
        }

        class RuneOfMisdirection : ActionPower
        {
            public RuneOfMisdirection()
            {
                Name = "Rune of Misdirection";
                Text = "Deactivate all Runes. Activate.";
                ActiveText = "Roll twice for Necromancer movement and choose one result.";
            }
        }

        class Divination : ActionPower
        {
            public Divination()
            {
                Name = "Divination";
                Text = "Exhaust to search with 2 dice in any location (not necessarily the location you are at).";
            }
        }

        class RuneOfClairvoyance : ActionPower
        {
            public RuneOfClairvoyance()
            {
                Name = "Rune of Clairvoyance";
                Text = "Deactivate all Runes. Activate.";
                ActiveText = "At the start of your turn, look at the top card of any deck; put it on the top or bottom of that deck.";
            }
        }
    }
}