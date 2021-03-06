﻿using System;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards.Enemies
{
    public class EnemyEventCard : IEventCard
    {
        public EnemyEventCard(string name, int fate, Action<EventDetail.EventDetailCreation> def)
        {
            Detail = EventDetail.Create(name, fate, def);
        }

        public EventDetail Detail { get; set; }

        public virtual void Resolve(Hero hero, string option)
        {
            if (option != "cont") return;
            var enemy = Detail.GetEnemyName(hero);
            hero.CurrentEvent = null;
            hero.DisplayCurrentEvent();
            hero.FaceEnemy(enemy);
        }
    }
}