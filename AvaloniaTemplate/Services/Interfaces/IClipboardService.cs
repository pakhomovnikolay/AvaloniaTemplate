using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IClipboardService
    {
        #region Удалить после вставки
        /// <summary>
        /// Удалить после вставки
        /// </summary>
        bool IsCut { get; set; }
        #endregion

        #region Копировать данные в буфер обмена
        /// <summary>
        /// Копировать данные в буфер обмена
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task CopyTextAsync(string text);
        #endregion

        #region Копировать данные в буфер обмена, с последующим удалением после вставки
        /// <summary>
        /// Копировать данные в буфер обмена, с последующим удалением после вставки
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task CutTextAsync(string text);
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