using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class HeroFactory : IHeroFactory
    {
        private static readonly Dictionary<string, HeroFactory> Factories = CreateFactories().ToDictionary(x => x.Name);
        private readonly int _grace;
        private readonly string[] _powers;
        private readonly int _secrecy;

        protected HeroFactory(string name, int grace, int secrecy, params string[] powers)
        {
            Name = name;
            _grace = grace;
            _secrecy = secrecy;
            _powers = powers;
        }

        public string Name { get; set; }

        public Hero Create()
        {
            var hero = new Hero
            {
                Name = Name,
                DefaultGrace = _grace,
                Grace = _grace,
                DefaultSecrecy = _secrecy,
                Secrecy = _secrecy
            };
            hero.PowerDeck.AddRange(_powers);
            var defaultActions = new IAction[]
                {new StartTurn(), new Travel(), new Hide(), new Attack(), new Search(), new AttackNecromancer(), new EndTurn(),};
            foreach (var action in defaultActions)
                hero.AddAction(action);
            var defaultTactics = new ITactic[] {new BasicFightTactic(), new BasicEludeTactic()};
            foreach (var tactic in defaultTactics)
                hero.AddTactic(tactic);
            return hero;
        }

        private static IEnumerable<HeroFactory> CreateFactories()
        {
            yield return new HeroFactory("Acolyte", 3, 7,
                "Blinding Black", "Call to Death", "Dark Veil", "Death Mask", "Fade to Black", "False Life", "False Orders", "Final Rest", "Forbidden Arts", "Leech Life");
            yield return new HeroFactory("Druid", 5, 6,
                "Animal Companion", "Camouflage", "Celerity", "Raven Form", "Sprite Form", "Tree Form", "Tranquility", "Vines", "Visions", "Wolf Form");
            yield return new HeroFactory("Knight", 5, 6, "Charge", "Consecrated Blade", "Hard Ride", "Holy Mantle", "Oath of Defense", "Oath of Purging",
                "Oath of Valor", "Oath of Vengeance", "Reckless Abandon", "Sprint");
            yield return new HeroFactory("Priest", 6, 6, "Benediction", "Blessing of Faith", "Blessing of Piety", "Blessing of Strength", "Blessing of Wisdom", "Calm",
                "Censure", "Intercession", "Miracle", "Sanctuary");
            yield return new HeroFactory("Prince", 4, 3, "Chapel", "Divine Right", "Inspire", "Loyalty", "Rebellion", "Resistance", "Safe House", "Scouts",
                "Secret Passage", "Strategy");
            yield return new HeroFactory("Rogue", 4, 7, "Ambush", "Contacts", "Diversion", "Eavesdrop", "Sabotage", "Sap", "Shadow Cloak", "Skulk",
                "Stealth", "Vanish");
            yield return new HeroFactory("Scholar", 4, 6, "Find Weakness", "Foresight", "Preparation", "Thoroughness", "Ancient Charm", "Ancient Defense",
                "Ancient Sword", "Counterspell", "Forgotten Sanctuary", "Research Materials");
            yield return new HeroFactory("Seer", 4, 6, "Destiny", "Dowse", "Foreknowledge", "Hope", "Prediction", "Premonition", "Prophecy of Doom",
                "Prophecy of Fortune", "Prophecy of Safety", "Prophecy of Sanctuary");
            yield return new HeroFactory("Wizard", 3, 5, "Arcane Energy", "Divination", "Fiendfire", "Invisibility", "Lightning Strike", "Rune of Clairvoyance",
                "Rune of Interference", "Rune of Misdirection", "Rune of Nullification", "Teleport");
        }

        public static Hero Create(string heroName)
        {
            if (!Factories.ContainsKey(heroName))
                throw new ArgumentOutOfRangeException(nameof(heroName), heroName);
            return Factories[heroName].Create();
        }
    }
}