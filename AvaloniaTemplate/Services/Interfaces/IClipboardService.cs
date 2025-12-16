using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IClipboardService
    {
        #region Копировать данные в буфер обмена
        /// <summary>
        /// Копировать данные в буфер обмена
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task CopyTextAsync(string text);
        #endregion

        #region Вставить данные из буфера обмена
        /// <summary>
        /// Вставить данные из буфера обмена
        /// </summary>
        /// <returns></returns>
        Task<string> PasteTextAsync();
        #endregion

        #region Очисть буфер обмена
        /// <summary>
        /// Очисть буфер обмена
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
        #endregion

        #region Наличие данных в буфере обмена
        /// <summary>
        /// Наличие данных в буфере обмена
        /// </summary>
        /// <returns></returns>
        Task<bool> ContainsTextAsync();
        #endregion
    }
}
