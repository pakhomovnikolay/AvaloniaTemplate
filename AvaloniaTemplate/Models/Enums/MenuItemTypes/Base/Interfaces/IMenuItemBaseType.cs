using Avalonia;
using Avalonia.Media;

namespace AvaloniaTemplate.Models.Enums.MenuItemTypes.Base.Interfaces
{
    public interface IMenuItemBaseType
    {
        #region Заголовок
        /// <summary>
        /// Заголовок
        /// </summary>
        string Header { get; set; }
        #endregion

        #region Видимость разделителя
        /// <summary>
        /// Видимость разделителя
        /// </summary>
        bool IsVisibleSeparator { get; set; }
        #endregion

        #region Выбран
        /// <summary>
        /// Выбран
        /// </summary>
        bool IsChecked { get; set; }
        #endregion

        #region По умолчанию
        /// <summary>
        /// По умолчанию
        /// </summary>
        bool IsDefault { get; set; }
        #endregion

        #region Группа
        /// <summary>
        /// Группа
        /// </summary>
        string Group { get; set; }
        #endregion

        #region Стиль границ
        /// <summary>
        /// Стиль границ
        /// </summary>
        Thickness BorderThickness { get; set; }
        #endregion

        #region Цвет заднего фона
        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        IBrush Background { get; set; }
        #endregion

        #region Цвет границы
        /// <summary>
        /// Цвет границы
        /// </summary>
        IBrush BorderBrush { get; set; }
        #endregion

        #region Радиус скругления границ
        /// <summary>
        /// Радиус скругления границ
        /// </summary>
        CornerRadius CornerRadius { get; set; }
        #endregion
    }
}
