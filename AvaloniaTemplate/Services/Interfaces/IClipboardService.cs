using AvaloniaTemplate.Models.Enums;
using System;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    /// <summary>
    /// Сервис работы с буфером обмена
    /// </summary>
    public interface IClipboardService
    {
        #region Копировать данные в буфер обмена
        /// <summary>
        /// Копировать данные в буфер обмена
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clipboardType"></param>
        /// <exception cref="InvalidOperationException">Буфер обмена не доступенг</exception>
        /// <returns></returns>
        Task CopyToClipboardAsync(string buffer, ClipboardType clipboardType);
        #endregion

        #region Получить данные из буфера обмена
        /// <summary>
        /// Получить данные из буфера обмена
        /// </summary>
        /// <returns></returns>
        Task<string> GetFromClipboardAsync();
        #endregion

        #region Очисть буфер обмена
        /// <summary>
        /// Очисть буфер обмена
        /// </summary>
        /// <returns></returns>
        Task ClearClipboardAsync();
        #endregion

        #region Наличие данных в буфере обмена
        /// <summary>
        /// Наличие данных в буфере обмена
        /// </summary>
        /// <returns></returns>
        Task<bool> DataOnClipboardAsync();
        #endregion
    }
}