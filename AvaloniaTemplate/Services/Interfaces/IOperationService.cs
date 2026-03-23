using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    /// <summary>
    /// Сервис для реализации обработки данных в асинхронном оежиме
    /// </summary>
    public interface IOperationService
    {
        #region Выполняется
        /// <summary>
        /// Выполняется
        /// </summary>
        bool IsRunning { get; }
        #endregion

        #region Асинхронный запуск операций
        /// <summary>
        /// Асинхронный запуск операций
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task RunAsync(Func<CancellationToken, Task> operation);
        #endregion

        #region Отмена выполнения
        /// <summary>
        /// Отмена выполнения
        /// </summary>
        void Cancel();
        #endregion
    }
}
