﻿using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    public abstract class Item : IItem
    {
        protected Item(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public string Text { get; protected set; }
        public Hero Owner { get; set; }
    }
}