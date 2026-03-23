using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    /// <summary>
    /// Сервис записи лог файлй 
    /// </summary>
    public interface ILogService
    {
        #region Записать лог
        /// <summary>
        /// Записать лог
        /// </summary>
        /// <param name="log"></param>
        void Write(string log);
        #endregion

        #region Асинхронная запись лога
        /// <summary>
        /// Асинхронная запись лога
        /// </summary>
        /// <param name="log"></param>
        Task WriteAsync(string log);
        #endregion
    }
}
