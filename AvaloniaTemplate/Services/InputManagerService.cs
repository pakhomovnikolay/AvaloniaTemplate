using Avalonia.Input;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Services
{
    public class InputManagerService : IInputManagerService
    {
        private readonly IUIConnectorService connectorService = App.GetService<IUIConnectorService>();
        private readonly IScrollBarService scrollBarService = App.GetService<IScrollBarService>();
        private readonly IZoomService zoomService = App.GetService<IZoomService>();

        #region Обработка клавиш
        /// <summary>
        /// Обработка клавиш
        /// </summary>
        public void KeysHandler(object? sender, KeyEventArgs e)
        {
            //if (IsInput(e))
            //    stateService.AppActiveMode = AppActiveModeType.IsInput;
            //else
            //{
            //    if (IsClipboard(e))
            //        stateService.AppActiveMode = AppActiveModeType.Clipboard;
            //    else if (IsNavigation(e))
            //        stateService.AppActiveMode = AppActiveModeType.Navigation;
            //    else if (IsInputMode(e))
            //        stateService.AppActiveMode = AppActiveModeType.IsEditCell;
            //    else if (IsCancel(e))
            //        stateService.AppActiveMode = AppActiveModeType.Unknown;
            //}
        }
        #endregion

        #region Обработка колеса прокрутки
        /// <summary>
        /// Обработка колеса прокрутки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PointerWheelHandler(object? sender, PointerWheelEventArgs e)
        {
            if (e.Properties.IsLeftButtonPressed || e.Properties.IsRightButtonPressed)
                return;

            if (IsZoom(e))
                zoomService.RecalculateScale(e.Delta.Y);
            else
            {
                if (IsHorizontalScrollBar(e))
                    scrollBarService.UpdateHorizontalScrollBarValue(e.Delta.Y);
                else
                    scrollBarService.UpdateVerticalScrollBarValue(e.Delta.Y);
            }
            e.Handled = true;
        }
        #endregion

        #region Проверка что нажатая клавиша инициплизптор ввода
        /// <summary>
        /// Проверка что нажатая клавиша инициплизптор ввода
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool IsInput(KeyEventArgs e) => false;
        //=> stateService.AppActiveMode != AppActiveModeType.IsInput
        //    && stateService.AppActiveMode != AppActiveModeType.IsEditCell
        //    && e.KeyModifiers == KeyModifiers.None
        //    && (e.Key >= Key.A && e.Key <= Key.Z
        //    || e.Key == Key.Space
        //    || e.Key == Key.Back
        //    || e.Key >= Key.D0 && e.Key <= Key.D9
        //    || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9
        //    || e.Key == Key.Oem3 || e.Key == Key.Oem4
        //    || e.Key == Key.OemMinus || e.Key == Key.Subtract
        //    || e.Key == Key.OemPlus || e.Key == Key.Add
        //    || e.Key == Key.Divide
        //    || e.Key == Key.Multiply
        //    || e.Key == Key.Decimal
        //    || e.Key == Key.OemCloseBrackets
        //    || e.Key == Key.OemPipe
        //    || e.Key == Key.OemSemicolon
        //    || e.Key == Key.OemQuotes
        //    || e.Key == Key.OemComma
        //    || e.Key == Key.OemPeriod
        //    || e.Key == Key.OemQuestion);

        #endregion

        #region Проверка необходимости обработки буфера обмена
        /// <summary>
        /// Проверка необходимости обработки буфера обмена
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsClipboard(KeyEventArgs e)
            => e.KeyModifiers == KeyModifiers.Control
            && (e.Key == Key.C || e.Key == Key.X || e.Key == Key.V);
        #endregion

        #region Проверка возможности навигации по клавишам
        /// <summary>
        /// Проверка возможности навигации по клавишам
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool IsNavigation(KeyEventArgs e) => false;
        //{
        //    return stateService.AppActiveMode != AppActiveModeType.IsInput
        //        && (e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down)
        //        || e.Key == Key.Tab || e.Key == Key.Return
        //        || (e.Key == Key.Delete
        //            && stateService.AppActiveMode != AppActiveModeType.IsInput
        //            && stateService.AppActiveMode != AppActiveModeType.IsEditCell);
        //}
        #endregion

        #region Проверка что нажатая клавиша - вход в режим редактирования
        /// <summary>
        /// Проверка что нажатая клавиша - вход в режим редактирования
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsInputMode(KeyEventArgs e)
            => e.Key == Key.F2;
        #endregion

        #region Проверка что нажатая клавиша - отмена
        /// <summary>
        /// Проверка что нажатая клавиша - отмена
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool IsCancel(KeyEventArgs e)
            => e.Key == Key.Escape;
        #endregion

        #region Проверка на необходимость изменения масштаба
        /// <summary>
        /// Проверка на необходимость изменения масштаба
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsZoom(PointerWheelEventArgs e)
            => e.KeyModifiers.HasFlag(KeyModifiers.Control);
        #endregion

        #region Проверка на необходимость прокрутки положения полосы прокрутки
        /// <summary>
        /// Проверка на необходимость прокрутки положения полосы прокрутки
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsHorizontalScrollBar(PointerWheelEventArgs e)
            => e.KeyModifiers.HasFlag(KeyModifiers.Shift);
        #endregion
    }
}
