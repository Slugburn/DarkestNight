using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class EnemyEventCard : IEventCard
    {
        private readonly Func<Hero, int> _designator;

        public EnemyEventCard(string name, int fate, Func<Hero, int> designator, Action<EventDetail.EventDetailCreation> def)
        {
            _designator = designator;
            Detail = EventDetail.Create(name,fate,  def);
        }

        protected EnemyEventCard(string name, int fate, Action<EventDetail.EventDetailCreation> def)
        {
            Detail = EventDetail.Create(name, fate, def);
        }

        public EventDetail Detail { get; set; }

        public void Resolve(Hero hero, string option)
        {
            if (option != "cont") return;
            var enemy = GetEnemy(hero);
            hero.EndEvent();
            hero.FaceEnemy(enemy);
        }

        private string GetEnemy(Hero hero)
        {
            string enemy;
            if (_designator != null)
            {
                var value = _designator(hero);
                enemy = Detail.GetEnemies().Single(x => x.Min <= value && value <= x.Max).Name;
            }
            else
            {
                enemy = Detail.GetEnemies().Single().Name;
            }
            return enemy;
        }
    }
}