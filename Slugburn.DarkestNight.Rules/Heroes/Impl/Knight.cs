using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Knight : Hero
    {
        public Knight()
            : base(
                "Knight", 5, 6, new Charge(), new ConsecratedBlade(), new HardRide(), new HolyMantle(), new OathOfDefense(), new OathOfPurging(),
                new OathOfValor(), new OathOfVengeance(), new RecklessAbandon(), new Sprint())
        {
        }

        class Charge : TacticPower
        {
            public Charge() : base(TacticType.Fight)
            {
                Name = "Charge";
                StartingPower = true;
                Text = "Fight with 2 dice.";
            }
        }

        class ConsecratedBlade : Bonus
        {
            public ConsecratedBlade()
            {
                Name = "Consecrated Blade";
                Text = "+1 dice in fights.";
            }
        }

        class HardRide : ActionPower
        {
            public HardRide()
            {
                Name = "Hard Ride";
                StartingPower = true;
                Text = "Move twice, but gain no Secrecy.";
            }
        }

        class HolyMantle : Bonus
        {
            public HolyMantle()
            {
                Name = "Holy Mantle";
                Text = "+1 to default Grace. Add 1 to each die when praying.";
            }
        }

        interface IOath
        {
            string FulfillText { get; }
            string BreakText { get; }
        }

        class OathOfDefense : ActionPower, IOath
        {
            public OathOfDefense()
            {
                Name = "Oath of Defense";
                Text = "If no Oaths are active, activate until you fulfill or break.";
                ActiveText = "Gain 1 Grace (up to default) at start of turn.";
            }

            public string FulfillText => "No blights at location; You gain 1 Grace.";
            public string BreakText => "Leave location; you lose all Grace.";
        }

        class OathOfPurging: ActionPower, IOath
        {
            public OathOfPurging()
            {
                Name = "Oath of Purging";
                StartingPower = true;
                Text = "If no Oaths are active, activate until you fulfill or break.";
                ActiveText = "+2 dice in fights when attacking blights.";
            }

            public string FulfillText => "Destroy a blight; you gain 1 Grace.";
            public string BreakText => "Enter the Monastery; you lose 1 Grace.";
        }

        class OathOfValor: ActionPower, IOath
        {
            public OathOfValor()
            {
                Name = "Oath of Valor";
                Text = "If no Oaths are active, activate until you fulfill or break.";
                ActiveText = "+1 die in fights.";
            }

            public string FulfillText => "Win a fight; You may activate any Oath immediately.";
            public string BreakText => "Attempt to elude; you lose 1 Grace.";
        }

        class OathOfVengeance: ActionPower, IOath
        {
            public OathOfVengeance()
            {
                Name = "Oath of Vengeance";
                Text = "If no Oaths are active, activate until you fulfill or break.";
                ActiveText = "Add 1 to highest die when fighting the Necormancer.";
            }

            public string FulfillText => "Win fight versus the Necromancer; you get a free action.";
            public string BreakText => "Hide or search; you lose 1 Grace.";
        }

        class RecklessAbandon: TacticPower
        {
            public RecklessAbandon() : base(TacticType.Fight)
            {
                Name = "Reckless Abandon";
                Text = "Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.";
            }
        }

        class Sprint : TacticPower
        {
            public Sprint() : base(TacticType.Elude)
            {
                Name = "Sprint";
                StartingPower = true;
                Text = "Elude with 2 dice.";
            }
        }
    }
}
