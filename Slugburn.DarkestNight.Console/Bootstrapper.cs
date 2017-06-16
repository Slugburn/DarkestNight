using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Slugburn.DarkestNight.Ninject;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight
{
    public static class Bootstrapper
    {
        public static StandardKernel Configure()
        {
            var kernel = new StandardKernel();
            kernel.Load<HeroModule>();
            kernel.Bind<IPlayer>().To<ConsolePlayer>();
            return kernel;
        }
    }

    public class ConsolePlayer : IPlayer
    {
        public ICollection<IHero> ChooseHeroes(IEnumerable<IHero> availableHeroes)
        {
            var available = availableHeroes.ToList();
            var selected = new List<IHero>();
            while (selected.Count < 4)
            {
                Console.WriteLine($"Choose Hero #{selected.Count+1}");
                Enumerable.Range(1,available.Count).ToList()
                    .ForEach(x=>Console.WriteLine($"{x}. {available[x-1].Name}"));
                var input = Console.ReadLine();
                if (input == null) continue;
                var selection = int.Parse(input);
                var hero = available[selection - 1];
                selected.Add(hero);
                available.Remove(hero);
                Console.WriteLine($"Choose powers for {hero.Name}:");
                var startingPowers = hero.Powers.Where(x => x.StartingPower).ToList();
                Enumerable.Range(1,startingPowers.Count).ToList()
                    .ForEach(x=>Console.WriteLine($"{x}. {startingPowers[x-1].Name} ({startingPowers[x-1].Text})"));
//                var selectedPowers = 
            }
            return selected;
        }
    }
}
