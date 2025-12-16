using Avalonia.Threading;
using System;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public static class CommandManager
    {
        #region Событие подписки\отписки наблюдателя
        /// <summary>
        /// Событие подписки\отписки наблюдателя
        /// </summary>
        public static event EventHandler RequerySuggested;
        #endregion

        #region Анализ возможности выполнения команды
        /// <summary>
        /// Анализ возможности выполнения команды. Выполняем только в соем потоке
        /// </summary>
        public static void InvalidateRequireSuggested()
        {
            var handler = RequerySuggested;
            if (handler is null)
                return;

            if (Dispatcher.UIThread.CheckAccess())
                Dispatcher.UIThread.Post(() => handler(null, EventArgs.Empty));
            else
                handler(null, EventArgs.Empty);
        }
        #endregion
    }
}
