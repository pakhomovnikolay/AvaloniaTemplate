using AvaloniaTemplate.Models.Enums;
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
        /// <param name="clipboardType"></param>
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