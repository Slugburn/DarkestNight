using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    internal class IgnoreBlight : IIgnoreBlight
    {
        public static IIgnoreBlight Create(string name, Hero hero)
        {
            return new IgnoreBlight {Name = name, HeroName = hero.Name};
        }

        public string HeroName { get; set; }
        public string Name { get; set; }
        public bool IsIgnoring(Hero hero, Blight blight)
        {
            return hero.Name == HeroName;
        }
    }
}