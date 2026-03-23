using Avalonia.Input.Platform;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class ClipboardService : IClipboardService
    {
        #region Системный буфер обмена
        /// <summary>
        /// Системный буфер обмена
        /// </summary>
        private static IClipboard Clipboard =>
            App.GetTopLevel()?.Clipboard
            ?? throw new InvalidOperationException("Clipboard недоступен");
        #endregion

        #region Текущий тип копирования данных
        /// <summary>
        /// Текущий тип обмена
        /// </summary>
        private ClipboardType currentClipboardType = ClipboardType.Unknown;
        #endregion

        #region Копировать данные в буфер обмена
        /// <summary>
        /// Копировать данные в буфер обмена
        /// </summary>
        /// <param name="text"></param>
        /// <param name="clipboardType"></param>
        /// <returns></returns>
        public async Task CopyToClipboardAsync(string buffer, ClipboardType clipboardType)
        {
            if (string.IsNullOrWhiteSpace(buffer))
                return;

            currentClipboardType = clipboardType;
            await CopyToSystemClipboardAsync(buffer);
        }
        #endregion

        #region Получить данные из буфера обмена
        /// <summary>
        /// Получить данные из буфера обмена
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetFromClipboardAsync()
        {
            var buffer = await Clipboard.TryGetTextAsync();
            if (currentClipboardType == ClipboardType.Cut)
                await ClearClipboardAsync();

            return buffer;
        }
        #endregion

        #region Очисть буфер обмена
        /// <summary>
        /// Очисть буфер обмена
        /// </summary>
        /// <returns></returns>
        public async Task ClearClipboardAsync()
        {
            await Clipboard.ClearAsync();
            currentClipboardType = ClipboardType.Unknown;
        }
        #endregion

        #region Наличие данных в буфере обмена
        /// <summary>
        /// Наличие данных в буфере обмена
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DataOnClipboardAsync()
            => !string.IsNullOrWhiteSpace(await Clipboard.TryGetTextAsync());
        #endregion

        #region Копируем данные в системный буфер обмена
        /// <summary>
        /// Копируем данные в системный буфер обмена
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static async Task CopyToSystemClipboardAsync(string buffer)
        {
            await Clipboard.SetTextAsync(buffer);
        }
        #endregion

    }
}
