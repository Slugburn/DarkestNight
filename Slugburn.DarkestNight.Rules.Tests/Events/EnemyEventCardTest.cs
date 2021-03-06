﻿using System;
using System.Linq;
using NUnit.Framework;
using Slugburn.DarkestNight.Rules.Tests.Fluent;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Events
{
    [TestFixture]
    public class EnemyEventCardTest
    {
        [TestCase(0, "Archer")]
        [TestCase(1, "Archer")]
        [TestCase(2, "Lich")]
        [TestCase(3, "Lich")]
        [TestCase(4, "Reaper")]
        public void BlackBanner(int blightCount, string target)
        {
            var blights = Enumerable.Repeat("Desecration", blightCount).ToArray();
            TestMultipleTypeEnemyGeneratorEvent("Black Banner", target, "Count the blights in your location", 4,
                given => given.Hero().At("Village").Location("Village").HasBlights(blights));
        }

        [TestCase(0, "Ghoul")]
        [TestCase(9, "Ghoul")]
        [TestCase(10, "Revenant")]
        [TestCase(19, "Revenant")]
        [TestCase(20, "Slayer")]
        public void DarkChampion(int darkness, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Dark Champion", enemy, "Compare to Darkness", 3,
                given => given.Game.Darkness(darkness));
        }

        [TestCase(5, "Scout")]
        [TestCase(3, "Archer")]
        [TestCase(4, "Archer")]
        [TestCase(2, "Dread")]
        [TestCase(0, "Dread")]
        public void DeadServant(int secrecy, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Dead Servant", enemy, "Compare to Secrecy", 3,
                given => given.Hero().Secrecy(secrecy));
        }

        [TestCase(6, "Flying Demon")]
        [TestCase(5, "Fearful Demon")]
        [TestCase(4, "Fearful Demon")]
        [TestCase(3, "Deadly Demon")]
        [TestCase(0, "Deadly Demon")]
        public void Demon(int secrecy, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Demon", enemy, "Compare to Secrecy", 3,
                given => given.Hero().Secrecy(secrecy));
        }

        //
        // GuardedTrove has its own test fixture
        //

        [TestCase(4, "Small Horde")]
        [TestCase(3, "Large Horde")]
        [TestCase(2, "Large Horde")]
        [TestCase(1, "Giant Horde")]
        [TestCase(0, "Giant Horde")]
        public void Horde(int secrecy, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Horde", enemy, "Compare to Secrecy", 3,
                given => given.Hero().Secrecy(secrecy));
        }

        [TestCase(0, "Archer")]
        [TestCase(14, "Archer")]
        [TestCase(15, "Lich")]
        [TestCase(24, "Lich")]
        [TestCase(25, "Reaper")]
        public void Patrols(int darkness, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Patrols", enemy, "Compare to Darkness", 4,
                given => given.Game.Darkness(darkness));
        }

        [TestCase(6, "Zombie")]
        [TestCase(5, "Mummy")]
        [TestCase(4, "Mummy")]
        [TestCase(3, "Slayer")]
        [TestCase(0, "Slayer")]
        public void ShamblingHorror(int secrecy, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Shambling Horror", enemy, "Compare to Secrecy", 4,
                given => given.Hero().Secrecy(secrecy));
        }

        [TestCase(5, "Shade")]
        [TestCase(4, "Shadow")]
        [TestCase(3, "Shadow")]
        [TestCase(2, "Hunter")]
        [TestCase(0, "Hunter")]
        public void VengefulSpirit(int secrecy, string enemy)
        {
            TestMultipleTypeEnemyGeneratorEvent("Vengeful Spirit", enemy, "Compare to Secrecy", 4,
                given => given.Hero().Secrecy(secrecy));
        }

        private static void TestSingleTypeEnemyGeneratorEvent(string eventName, string enemy, int expectedFate)
        {
            TestScenario
                .Game.WithHero()
                .Given.Hero().HasDrawnEvent(eventName)
                .Then(Verify.Player.EventView.HasBody(eventName, expectedFate, null).HasOptions("Continue"))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero().HasUnresolvedEvents(0).Secrecy(null))
                .Then(Verify.Player.ConflictModel.HasTargets(enemy));
        }

        private static void TestMultipleTypeEnemyGeneratorEvent(string eventName, string enemy, string text, int expectedFate, Func<IGiven, IGiven> designator)
        {
            designator = designator ?? (s => s);
            TestScenario
                .Game.WithHero()
                .Given.Configure(designator)
                .Given.Hero().HasDrawnEvent(eventName)
                .Then(Verify.Player.EventView.HasBody(eventName, expectedFate, text).HasOptions("Continue").ActiveRow(enemy))
                .When.Player.SelectsEventOption("Continue")
                .Then(Verify.Hero().HasUnresolvedEvents(0).Secrecy(null))
                .Then(Verify.Player.ConflictModel.HasTargets(enemy))
                .Then(Verify.Player.Hero().Commands.Exactly());
        }

        [Test]
        public void Cultist()
        {
            TestSingleTypeEnemyGeneratorEvent("Cultist", "Cultist", 1);
        }

        [Test]
        public void Lich()
        {
            TestSingleTypeEnemyGeneratorEvent("Lich", "Lich", 4);
        }

        [Test]
        public void Looters()
        {
            TestSingleTypeEnemyGeneratorEvent("Looters", "Looters", 2);
        }

        [Test]
        public void Tracker()
        {
            TestSingleTypeEnemyGeneratorEvent("Tracker", "Tracker", 5);
        }

        [Test]
        public void VileMessenger()
        {
            TestSingleTypeEnemyGeneratorEvent("Vile Messenger", "Vile Messenger", 4);
        }
    }
}