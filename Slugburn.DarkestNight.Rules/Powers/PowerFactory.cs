using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public class PowerFactory
    {
        private static readonly Dictionary<string, Func<IPower>> Activators;

        static PowerFactory()
        {
            Activators = typeof(PowerFactory).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IPower).IsAssignableFrom(t))
                .ToDictionary(t => CreateInstance(t).Name, t => (Func<IPower>)(() => CreateInstance(t)));
        }

        private static IPower CreateInstance(Type t)
        {
            var power = (IPower)Activator.CreateInstance(t);
            if (power.Name==null)
                throw new Exception(t.FullName);
            return power;
        }

        public static IPower Create(string powerName)
        {
            if (!Activators.ContainsKey(powerName))
                throw new ArgumentOutOfRangeException(nameof(powerName), powerName, "No activator is available for the requested name.");
            return Activators[powerName]();
        }
    }
}
