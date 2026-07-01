using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class FontFamilyHelper
    {
        static FontFamilyHelper()
        {
            ControlPreviewFontProperty.Changed.AddClassHandler<FontSelectorToolkit>(RegistryPreviewFontChanged);
            ControlFontChangedProperty.Changed.AddClassHandler<FontSelectorToolkit>(RegistryControlFontChanged);
            ControlPreviewFontSizeProperty.Changed.AddClassHandler<FontSelectorToolkit>(RegistryPreviewFonSizeChanged);
            ControlFontSizeCHangedProperty.Changed.AddClassHandler<FontSelectorToolkit>(RegistryControlFontSizeCHanged);
        }

        #region Список шрифтов
        /// <summary>
        /// Список шрифтов
        /// </summary>
        public static List<FontFamily> FontFamilies { get; } = FontManager.Current.SystemFonts?.ToList() ?? [];
        #endregion

        #region Шрифт по умолчнию
        /// <summary>
        /// Шрифт по умолчнию
        /// </summary>
        public static FontFamily FontDefault { get; } = FontManager.Current.DefaultFontFamily ?? "";
        #endregion

        #region Шрифт приложения по умолчнию
        /// <summary>
        /// Шрифт приложения по умолчнию
        /// </summary>
        public static FontFamily AppFontDefault { get; } = Helper.GetResource<FontFamily>("FontFamilyDefault");
        #endregion

        #region Размеры шрифта
        /// <summary>
        /// Размеры шрифта
        /// </summary>
        public static List<double> FontSizes { get; } = [8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36, 48, 72];
        #endregion

        #region Размер шрифта по умолчнию
        /// <summary>
        /// Размер шрифта по умолчнию
        /// </summary>
        public static double FontSizeDefault { get; } = Helper.GetResource<double>("FontSizeDefault") == 0
            ? 11
            : Helper.GetResource<double>("FontSizeDefault");
        #endregion

        #region Контроль предпросмотра выбора шрифта
        /// <summary>
        /// Контроль предпросмотра выбора шрифта
        /// </summary>
        public static readonly AttachedProperty<bool> ControlPreviewFontProperty =
            AvaloniaProperty.RegisterAttached<BorderHelper, FontSelectorToolkit, bool>("ControlPreviewFont");

        /// <summary>
        /// Установить контроль предпросмотра выбора шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControlPreviewFont(AvaloniaObject element, bool value)
            => element.SetValue(ControlPreviewFontProperty, value);

        /// <summary>
        /// Получить состояние контроля предпросмотра выбора шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetControlPreviewFont(AvaloniaObject element)
            => element.GetValue(ControlPreviewFontProperty);



        private static void RegistryPreviewFontChanged(FontSelectorToolkit selector, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
                selector.PreviewChanged += OnPreviewFontChanged;
            else
                selector.PreviewChanged -= OnPreviewFontChanged;
        }

        private static void OnPreviewFontChanged(object? item)
        {
            if (item is not { } || item is not FontFamily font)
                return;

            Debug.WriteLine(font.Name);

            //if (sender is not Border border)
            //    return;

            //var command = GetPointerEnteredCommand(border);
            //var commandParameter = GetPointerEnteredCommandParameter(border);
            //command?.Execute(commandParameter);

        }
        #endregion

        #region Контроль выбора шрифта
        /// <summary>
        /// Контроль выбора шрифта
        /// </summary>
        public static readonly AttachedProperty<bool> ControlFontChangedProperty = 
            AvaloniaProperty.RegisterAttached<BorderHelper, FontSelectorToolkit, bool>("ControlFontChanged");

        /// <summary>
        /// Установить контроль выбора шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControlFontChanged(AvaloniaObject element, bool value)
            => element.SetValue(ControlFontChangedProperty, value);

        /// <summary>
        /// Получить состояние контроля выбора шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetControlFontChanged(AvaloniaObject element)
            => element.GetValue(ControlFontChangedProperty);



        private static void RegistryControlFontChanged(FontSelectorToolkit selector, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
                selector.SelectedItemChanged += OnFontChanged;
            else
                selector.SelectedItemChanged -= OnFontChanged;
        }

        private static void OnFontChanged(object? item)
        {
            if (item is not { } || item is not FontFamily font)
                return;

            Debug.WriteLine(font.Name);

            //if (sender is not Border border)
            //    return;

            //var command = GetPointerEnteredCommand(border);
            //var commandParameter = GetPointerEnteredCommandParameter(border);
            //command?.Execute(commandParameter);

        }
        #endregion

        #region Контроль предпросмотра выбора размера шрифта
        /// <summary>
        /// Контроль предпросмотра выбора размера шрифта
        /// </summary>
        public static readonly AttachedProperty<bool> ControlPreviewFontSizeProperty =
            AvaloniaProperty.RegisterAttached<BorderHelper, FontSelectorToolkit, bool>("ControlPreviewFontSize");

        /// <summary>
        /// Установить контроль предпросмотра выбора размера шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControlPreviewFontSize(AvaloniaObject element, bool value)
            => element.SetValue(ControlPreviewFontSizeProperty, value);

        /// <summary>
        /// Получить состояние контроля предпросмотра выбора размера шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetControlPreviewFontSize(AvaloniaObject element)
            => element.GetValue(ControlPreviewFontSizeProperty);



        private static void RegistryPreviewFonSizeChanged(FontSelectorToolkit selector, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
                selector.PreviewChanged += OnPreviewFonSizeChanged;
            else
                selector.PreviewChanged -= OnPreviewFonSizeChanged;
        }

        private static void OnPreviewFonSizeChanged(object? item)
        {
            if (item is not { } || item is not double size)
                return;

            Debug.WriteLine(size);

            //if (sender is not Border border)
            //    return;

            //var command = GetPointerEnteredCommand(border);
            //var commandParameter = GetPointerEnteredCommandParameter(border);
            //command?.Execute(commandParameter);

        }
        #endregion

        #region Контроль выбора размера шрифта
        /// <summary>
        /// Контроль выбора размера шрифта
        /// </summary>
        public static readonly AttachedProperty<bool> ControlFontSizeCHangedProperty =
            AvaloniaProperty.RegisterAttached<BorderHelper, FontSelectorToolkit, bool>("ControlFontSizeCHanged");

        /// <summary>
        /// Установить контроль выбора размера шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControlFontSizeCHanged(AvaloniaObject element, bool value)
            => element.SetValue(ControlFontSizeCHangedProperty, value);

        /// <summary>
        /// Получить состояние контроля выбора размера шрифта
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetControlFontSizeCHanged(AvaloniaObject element)
            => element.GetValue(ControlFontSizeCHangedProperty);



        private static void RegistryControlFontSizeCHanged(FontSelectorToolkit selector, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
                selector.SelectedItemChanged += OnFontSizeCHanged;
            else
                selector.SelectedItemChanged -= OnFontSizeCHanged;
        }

        private static void OnFontSizeCHanged(object? item)
        {
            if (item is not { } || item is not double size)
                return;

            Debug.WriteLine(size);

            //if (sender is not Border border)
            //    return;

            //var command = GetPointerEnteredCommand(border);
            //var commandParameter = GetPointerEnteredCommandParameter(border);
            //command?.Execute(commandParameter);

        }
        #endregion
    }
}
