using System.Windows.Input;
using Slugburn.DarkestNight.Rules;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class EventOptionVm
    {
        public EventOptionVm(Game game, EventOptionModel model)
        {
            Text = model.Text;
            Command = new CommandHandler(
                () => game.ActingHero.SelectEventOption(model.Code));
        }

        public EventOptionVm()
        {
        }

        public static EventOptionVm Create(Game game, EventOptionModel model)
        {
            return new EventOptionVm(game, model);
        }

        public string Text { get; set; }

        public ICommand Command { get; set; }
    }
}