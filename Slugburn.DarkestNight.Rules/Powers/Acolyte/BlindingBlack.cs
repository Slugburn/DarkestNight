using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    public class BlindingBlack : Bonus
    {
        public BlindingBlack()
        {
            Name = "Blinding Black";
            StartingPower = true;
            Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            var handler = new BlindingBlackTriggerHandler {HeroName = hero.Name};
            hero.Game.Triggers.Add(GameTrigger.NecromancerDetectsHeroes, Name, handler);
        }

        public class BlindingBlackTriggerHandler : ITriggerHandler<Game>
        {
            public string HeroName { get; set; }

            public void HandleTrigger(Game registrar, string source, TriggerContext context)
            {
                var hero = registrar.GetHero(HeroName);
                var power = hero.GetPower(source);
                if (!power.IsUsable(hero)) return;
                if (!hero.Player.AskUsePower(source, power.Text)) return;
                power.Exhaust(hero);
                context.Cancel = true;
            }
        }
    }
}