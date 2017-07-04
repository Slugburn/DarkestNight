using System;
using System.Windows.Input;

namespace Slugburn.DarkestNight.Wpf
{
    public class CommandHandler : ICommand
    {
        private readonly Action _action;

        public CommandHandler(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
