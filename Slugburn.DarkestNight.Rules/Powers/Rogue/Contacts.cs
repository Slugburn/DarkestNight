using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Rogue
{
    internal class Contacts : BonusPower
    {
        public Contacts()
        {
            Name = "Contacts";
            StartingPower = true;
            Text = "Exhaust at any time to gain 1 Secrecy (up to 7).";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Secrecy < 7;
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new ContactsCommand(this));
        }

        private class ContactsCommand : PowerCommand
        {
            public ContactsCommand(IPower power) : base(power, false) { }

            public override void Execute(Hero hero)
            {
                Power.Exhaust(hero);
                hero.GainSecrecy(1, 7);
            }
        }
    }
}