using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class AnimalCompanion : TacticPower
    {
        public AnimalCompanion() : base(TacticType.Fight)
        {
            Name = "Animal Companion";
            StartingPower = true;
            Text = "Fight with 2 dice. Exhaust if you fail.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new AnimalCompanionTactic());
        }

        private class AnimalCompanionTactic : PowerTactic
        {
            public AnimalCompanionTactic()
            {
                PowerName = "Animal Companion";
                Type = TacticType.Fight;
                DiceCount = 2;
            }
        }
    }
}