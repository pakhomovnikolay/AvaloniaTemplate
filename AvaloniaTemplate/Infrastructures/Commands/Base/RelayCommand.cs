using System;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    #region Команда для неопределнных типов
    /// <summary>
    /// Команда для неопределнных типов
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public class RelayCommand(Action<object?> execute, Func<object?, bool> canExecute = null) : Command
    {
        private readonly Action<object?> _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<object?, bool> _CanExecute = canExecute;

        protected override bool CanExecute(object? p) => _CanExecute?.Invoke(p) ?? true;
        protected override void Execute(object? p) => _Execute(p);

        public RelayCommand(Action Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? null : p => CanExecute())
        { }
    }
    #endregion

    #region Команда для обобщенных типов
    /// <summary>
    /// Команда для обобщенных типов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public class RelayCommand<T>(Action<T> execute, Func<T, bool> canExecute = null) : Command
    {
        private readonly Action<T> _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<T, bool> _CanExecute = canExecute;

        protected override bool CanExecute(object p)
             => p is T t ? _CanExecute?.Invoke(t) ?? true : _CanExecute?.Invoke(default) ?? true;

        protected override void Execute(object p)
        {
            if (p is T t)
                _Execute(t);
        }

        public RelayCommand(Action Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? null : p => CanExecute())
        { }
    }
    #endregion

    #region Асинхронная команда для неопределнных типов
    /// <summary>
    /// Асинхронная команда для неопределнных типов
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public class RelayCommandAsync(Func<object?, Task> execute, Func<object?, bool> canExecute = null) : Command
    {
        private readonly Func<object?, Task> _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<object?, bool> _CanExecute = canExecute;
        private bool _isRunning;

        protected override bool CanExecute(object? p)
             => !_isRunning && (_CanExecute?.Invoke(p) ?? true);
        protected override async void Execute(object? p)
        {
            _isRunning = true;
            CommandManager.InvalidateRequireSuggested();

            try { await _Execute(p); }
            finally
            {
                _isRunning = false;
                CommandManager.InvalidateRequireSuggested();
            }
        }

        public RelayCommandAsync(Func<Task> Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? null : p => CanExecute())
        { }
    }
    #endregion

    #region Асинхронная команда для обощенных типов
    /// <summary>
    /// Асинхронная команда для обощенных типов
    /// </summary>
    /// <param name="executeAsync"></param>
    /// <param name="canExecute"></param>
    public class RelayCommandAsync<T>(Func<T?, Task> execute, Func<T?, bool> canExecute = null) : Command
    {
        private readonly Func<T?, Task> _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<T?, bool> _CanExecute = canExecute;
        private bool _isRunning;

        protected override bool CanExecute(object? p)
             => !_isRunning && (p is T t ? _CanExecute?.Invoke(t) ?? true : _CanExecute?.Invoke(default) ?? true);
        protected override async void Execute(object? p)
        {
            _isRunning = true;
            CommandManager.InvalidateRequireSuggested();
            try
            {
                if (p is T t)
                    await _Execute(t);
            }
            finally
            {
                _isRunning = false;
                CommandManager.InvalidateRequireSuggested();
            }
        }

        public RelayCommandAsync(Func<Task> Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? null : p => CanExecute())
        { }
    }
    #endregion
}