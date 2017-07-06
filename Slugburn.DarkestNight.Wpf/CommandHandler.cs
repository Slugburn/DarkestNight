using System;
using System.Windows.Input;

namespace Slugburn.DarkestNight.Wpf
{
    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public CommandHandler(Action action) : this(action, ()=>true)
        {
        }

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
