using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Items
{
    public static class ItemFactory
    {
        private static Dictionary<string, Func<Item>> Activators { get; }

        static ItemFactory()
        {
            Activators = typeof(ItemFactory).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(Item).IsAssignableFrom(t))
                .ToDictionary(t => CreateInstance(t).Name, t => (Func<Item>)(() => CreateInstance(t)));
        }


        public static IItem Create(int id, string itemName)
        {
            if (!Activators.ContainsKey(itemName))
                throw new ArgumentOutOfRangeException(nameof(itemName), itemName, "No activator is available for the requested name.");
            var item = Activators[itemName]();
            item.Id = id;
            return item;
        }

        private static Item CreateInstance(Type t)
        {
            var item = (Item)Activator.CreateInstance(t);
            if (item.Name == null)
                throw new Exception(t.FullName);
            return item;
        }
    }
}
