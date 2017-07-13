using System;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class GuardedTroveEventCard : SingleEnemyEventCard
    {
        public GuardedTroveEventCard() : base("Guarded Trove", 1)
        {
        }

        public override void Resolve(Hero hero, string option)
        {
            switch (option)
            {
                case "cont":
                    var enemy = Detail.GetEnemyName(hero);
                    // unlike most enemy events, we may need to come back and choose between more options,
                    // so don't end event here
                    hero.FaceEnemy(enemy);
                    break;
                case "spend-secrecy":
                    hero.SpendSecrecy(1);
                    hero.EndEvent();
                    break;
                case "draw-event":
                    var name = hero.Game.Events.Draw();
                    hero.EventQueue.Enqueue(name);
                    hero.EndEvent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option);
            }
        }
    }
}