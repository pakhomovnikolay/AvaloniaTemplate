using System;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequireSuggested += value;
            remove => CommandManager.RequireSuggested -= value;
        }

        private bool pExecutable = true;
        public bool Executable
        {
            get => pExecutable;
            set => ControlCanExecute(value);
        }

        bool ICommand.CanExecute(object p) => pExecutable && CanExecute(p);
        void ICommand.Execute(object p)
        {
            if (CanExecute(p))
                Execute(p);
        }

        private void ControlCanExecute(bool value)
        {
            if (pExecutable == value) return;
            pExecutable = value;
            CommandManager.InvalidateRequireSuggested();
        }

        protected virtual bool CanExecute(object p) => true;
        protected abstract void Execute(object p);
    }
}
