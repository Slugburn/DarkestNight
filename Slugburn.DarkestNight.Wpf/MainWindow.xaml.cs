using System.Linq;
using System.Windows;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Heroes;
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
            var player = (Player)DataContext;
            player.Game = game;
            game.AddPlayer(player);
            game.PopulateInitialBlights();
            var heroes = new string[] { "Acolyte", "Druid", "Knight", "Priest" }.Select(HeroFactory.Create);
            foreach (var hero in heroes)
            {
                game.AddHero(hero, player);
                var startingPowers = hero.PowerDeck.Select(PowerFactory.Create).Where(x => x.StartingPower).Shuffle().Take(3).Select(x => x.Name).ToList();
                foreach (var powerName in startingPowers)
                    hero.LearnPower(powerName);
            }
            game.UpdatePlayerBoard();
        }
    }
}
