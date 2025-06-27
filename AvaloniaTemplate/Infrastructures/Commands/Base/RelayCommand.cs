using System;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public class RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : Command
    {
        private readonly Action<object> _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<object, bool> _CanExecute = canExecute;

        protected override bool CanExecute(object p) => _CanExecute?.Invoke(p) ?? true;
        protected override void Execute(object p) => _Execute(p);

        public RelayCommand(Action Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? null : p => CanExecute())
        { }
    }
}