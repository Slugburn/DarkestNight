﻿using System.IO;
using System.Linq;
using System.Windows;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.IO;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Wpf.ViewModels;

namespace Slugburn.DarkestNight.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var game = new Game();
            var player = (PlayerVm)DataContext;
            player.Game = game;
            game.AddPlayer(player);
            if (File.Exists("game.json"))
                LoadGame(game);
            else
                CreateGame(game, player);
            game.UpdatePlayerBoard();
            foreach (var hero in game.Heroes)
            {
                hero.UpdateHeroStatus();
                hero.UpdateAvailableCommands();
            }
        }

        private static void CreateGame(Game game, IPlayer player)
        {
            game.PopulateInitialBlights();
            var heroes = new[] {"Rogue", "Druid", "Knight", "Prince"}.Select(HeroFactory.Create);
            foreach (var hero in heroes)
            {
                game.AddHero(hero, player);
                var startingPowers = hero.PowerDeck.Select(PowerFactory.Create).Shuffle().Take(3).Select(x => x.Name).ToList();
                foreach (var powerName in startingPowers)
                    hero.LearnPower(powerName);
                hero.UpdateAvailableCommands();
            }
        }

        private static void LoadGame(Game game)
        {
            var serializer = new GameSerializer();
            using (var reader = File.OpenText("game.json"))
            {
                serializer.Read(game, reader);
            }
        }
    }
}
