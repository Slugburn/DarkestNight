using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Scholar : Hero
    {

        class FindWeakness : TacticPower
        {
            public FindWeakness() : base()
            {
                Name = "Find Weakness";
                StartingPower = true;
                Text = "Fight with 1 die. Before rolling, pick 1 die, and add 1 to its result.";
            }
        }

        class Foresight : TacticPower
        {
            public Foresight() : base()
            {
                Name = "Foresight";
                StartingPower = true;
                Text = "Elude with 2 dice.";
            }
        }

        class Preparation : BonusPower
        {
            public Preparation()
            {
                Name = "Preparation";
                StartingPower = true;
                Text = "Exhaust after you make any die roll to reroll it.";
            }
        }

        class Thoroughness: BonusPower
        {
            public Thoroughness()
            {
                Name = "Thoroughness";
                StartingPower = true;
                Text = "If you search successfully, draw an extra card (but still keep only 1).";
            }
        }

        class AncientCharm: ActionPower
        {
            public AncientCharm()
            {
                Name = "Ancient Charm";
                Text = "Activate in your location.";
                ActiveText = "When a hero has an event there, draw an extra card and discard 1.";
            }

        }

        class AncientDefense: ActionPower
        {
            public AncientDefense()
            {
                Name = "Ancient Defense";
                Text = "Activate in your location.";
                ActiveText = "When a blight appears there, draw an extra card and discard 1.";
            }

        }

        class AncientSword : BonusPower
        {
            public AncientSword()
            {
                Name = "Ancient Sword";
                Text = "+1 die in fights.";
            }
        }

        class Counterspell : ActionPower
        {
            public Counterspell()
            {
                Name = "Counterspell";
                Text = "Activate in your location.";
                ActiveText = "The might of blights there is reduced by 1.";
            }
        }

        class ForgottenSancutary : ActionPower
        {
            public ForgottenSancutary()
            {
                Name = "Forgotten Sanctuary";
                Text = "Activate in your location.";
                ActiveText = "Heroes gain +2 dice when eluding there.";
            }
        }

        class ResearchMaterials : BonusPower
        {
            public ResearchMaterials()
            {
                Name = "Research Materials";
                Text = "+1 die in searches.";
            }
        }
    }
}