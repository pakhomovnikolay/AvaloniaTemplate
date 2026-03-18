using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class OperationService : IOperationService
    {
        private CancellationTokenSource? _cts;

        #region Выполняется
        /// <summary>
        /// Выполняется
        /// </summary>
        public bool IsRunning => _cts is { };
        #endregion

        #region Асинхронный запуск операций
        /// <summary>
        /// Асинхронный запуск операций
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async Task RunAsync(Func<CancellationToken, Task> operation)
        {
            if (_cts is { })
                return;

            _cts = new CancellationTokenSource();

            try
            {
                await operation(_cts.Token);
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }
        #endregion

        #region Отмена выполнения
        /// <summary>
        /// Отмена выполнения
        /// </summary>
        public void Cancel()
            => _cts?.Cancel();
        #endregion
    }
}
