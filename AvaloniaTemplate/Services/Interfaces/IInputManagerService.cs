using Avalonia.Input;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IInputManagerService
    {
        #region Обработка клавиш
        /// <summary>
        /// Обработка клавиш
        /// </summary>
        void KeysHandler(object? sender, KeyEventArgs e);
        #endregion
    }
}
