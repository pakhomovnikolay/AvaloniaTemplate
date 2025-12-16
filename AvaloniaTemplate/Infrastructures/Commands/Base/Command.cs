using System;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public abstract class Command : ICommand
    {
        #region Событие подписки\отписки наблюдателя
        /// <summary>
        /// Событие подписки\отписки наблюдателя
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        #endregion

        #region Состояние возможности выполнения команды
        private bool executable = true;
        /// <summary>
        /// Состояние возможности выполнения команды
        /// </summary>
        public bool Executable
        {
            get => executable;
            set => ControlCanExecute(value);
        }
        #endregion

        #region Метод выполнения команды
        /// <summary>
        /// Метод выполнения команды
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object p) => executable && CanExecute(p);
        void ICommand.Execute(object p)
        {
            if (CanExecute(p))
                Execute(p);
        }
        #endregion

        #region Анализ возможности выполнения команды
        /// <summary>
        /// Анализ возможности выполнения команды
        /// </summary>
        /// <param name="value"></param>
        private void ControlCanExecute(bool value)
        {
            if (executable == value)
                return;

            executable = value;
            CommandManager.InvalidateRequireSuggested();
        }
        #endregion

        #region Метод для реализации контроля состояния возможности выполнения команды
        /// <summary>
        /// Метод для реализации контроля состояния возможности выполнения команды (выполнение разрешено)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected virtual bool CanExecute(object p) => true;
        #endregion

        #region Метод реализации действий по команде
        /// <summary>
        /// Метод реализации действий по команде (событие команды)
        /// </summary>
        /// <param name="p"></param>
        protected abstract void Execute(object p);
        #endregion

        #region Метод исполнения запуска события команды
        /// <summary>
        /// Метод исполнения запуска события команды (для доступа из класса команды)
        /// </summary>
        /// <param name="p"></param>
        public void Invoke(object p) => Execute(p);
        #endregion
    }
}
