using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Items
{
    public static class ItemFactory
    {
        private static Dictionary<string, Func<IItem>> Activators { get; }

        static ItemFactory()
        {
            Activators = typeof(ItemFactory).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IItem).IsAssignableFrom(t))
                .ToDictionary(t => CreateInstance(t).Name, t => (Func<IItem>)(() => CreateInstance(t)));
        }


        public static IItem Create(string itemName)
        {
            if (!Activators.ContainsKey(itemName))
                throw new ArgumentOutOfRangeException(nameof(itemName), itemName, "No activator is available for the requested name.");
            return Activators[itemName]();
        }

        private static IItem CreateInstance(Type t)
        {
            var item = (IItem)Activator.CreateInstance(t);
            if (item.Name == null)
                throw new Exception(t.FullName);
            return item;
        }
    }
}
