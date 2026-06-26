using Avalonia.Input;
using System;

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

        #region Обработка колеса прокрутки
        /// <summary>
        /// Обработка колеса прокрутки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PointerWheelHandler(object? sender, PointerWheelEventArgs e);
        #endregion
    }
}
