using Avalonia.Media;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class FontFamilyHelper
    {
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
    }
}
