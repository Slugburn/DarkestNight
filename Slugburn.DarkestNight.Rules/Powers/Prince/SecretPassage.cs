using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class SecretPassage : ActionPower
    {
        public SecretPassage()
        {
            Name = "Secret Passage";
            Text = "Move to an adjacent location and gain 2 Secrecy (up to 5).";
        }

        protected override void OnLearn()
        {
            base.OnLearn();
            Owner.AddCommand(new SecretPassageCommand(this));
        }

        private class SecretPassageCommand : PowerAction
        {
            public SecretPassageCommand(SecretPassage power) :base(power){}

            public override async void Execute(Hero hero)
            {
                hero.AvailableMovement = hero.TravelSpeed;
                await TravelHandler.UseAvailableMovement(hero);
                hero.GainSecrecy(2, 5);
            }
        }
    }
}