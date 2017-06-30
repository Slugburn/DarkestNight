﻿using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Rogue : Hero
    {

        class Ambush : TacticPower
        {
            public Ambush() : base()
            {
                Name = "Ambush";
                StartingPower = true;
                Text = "Spend 1 Secrecy to fight with 3 dice.";
            }
        }

        class Contacts : BonusPower
        {
            public Contacts()
            {
                Name = "Contacts";
                StartingPower = true;
                Text = "Exhaust at any time to gain 1 Secrecy (up to 7).";
            }
        }

        class Diversion : ActionPower
        {
            public Diversion()
            {
                Name = "Diversion";
                Text = "Spend 1 Secrecy to negate the effects of one blight in your location until the Necromancer ends a turn there.";
            }
        }

        class Eavesdrop : ActionPower
        {
            public Eavesdrop()
            {
                Name = "Eavesdrop";
                StartingPower = true;
                Text = "Spend 1 Secrecy to search with 2 dice.";
            }
        }

        class Sabotage : ActionPower
        {
            public Sabotage()
            {
                Name = "Sabotage";
                Text = "Spend 1 Secrecy in the Necromancer's location to cause -1 Darkness.";
            }
        }

        class Sap : BonusPower
        {
            public Sap()
            {
                Name = "Sap";
                Text = "Exhaust during your turn to reduce the might of a blight in your location by 1 until your next turn.";
            }
        }

        class ShadowCloak : BonusPower
        {
            public ShadowCloak()
            {
                Name = "Shadow Cloak";
                Text = "+1 die when eluding.";
            }
        }

        class Skulk : TacticPower
        {
            public Skulk() : base()
            {
                Name = "Skulk";
                Text = "Elude with 2 dice and add 1 to the highest die.";
            }
        }

        class Stealth : BonusPower
        {
            public Stealth()
            {
                Name = "Stealth";
                Text = "Any time you lose or spend Secrecy, you can spend 1 Grace instead.";
            }
        }

        class Vanish : TacticPower
        {
            public Vanish() : base()
            {
                Name = "Vanish";
                StartingPower = true;
                Text = "Elude with 2 dice. Gain 1 Secrecy (up to 7) if you roll 2 successes.";
            }
        }
    }

}