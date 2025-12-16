using Avalonia.Input.Platform;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services
{
    public class ClipboardService : IClipboardService
    {
        private static IClipboard Clipboard =>
            App.GetTopLevel()?.Clipboard
            ?? throw new InvalidOperationException("Clipboard недоступен");

        #region Копировать данные в буфер обмена
        /// <summary>
        /// Копировать данные в буфер обмена
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task CopyTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            await Clipboard.SetTextAsync(text);
        }
        #endregion

        #region Вставить данные из буфера обмена
        /// <summary>
        /// Вставить данные из буфера обмена
        /// </summary>
        /// <returns></returns>
        public async Task<string> PasteTextAsync()
            => await Clipboard.TryGetTextAsync()
                ?? string.Empty;
        #endregion

        #region Очисть буфер обмена
        /// <summary>
        /// Очисть буфер обмена
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
            => await Clipboard.ClearAsync();
        #endregion

        #region Наличие данных в буфере обмена
        /// <summary>
        /// Наличие данных в буфере обмена
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ContainsTextAsync()
        {
            var text = await Clipboard.TryGetTextAsync();
            return !string.IsNullOrWhiteSpace(text);
        }
        #endregion
    }
}
