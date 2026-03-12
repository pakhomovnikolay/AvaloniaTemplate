using Avalonia;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums.MenuItemTypes.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.Enums.MenuItemTypes.Base
{
    public class MenuItemBaseType : ObservableObject, IMenuItemBaseType
    {
        #region Заголовок
        private string header = "";
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }
        #endregion

        #region Видимость разделителя
        private bool visibleSeparator = false;
        /// <summary>
        /// Видимость разделителя
        /// </summary>
        public bool IsVisibleSeparator
        {
            get => visibleSeparator;
            set => SetProperty(ref visibleSeparator, value);
        }
        #endregion

        #region Видимость
        private bool visible = true;
        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsVisible
        {
            get => visible;
            set
            {
                if (SetProperty(ref visible, value))
                    if (!visible)
                        IsChecked = false;
            }
        }
        #endregion

        #region Выбран
        private bool isChecked = false;
        /// <summary>
        /// Выбран
        /// </summary>
        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
        #endregion

        #region По умолчанию
        private bool isDefault = false;
        /// <summary>
        /// По умолчанию
        /// </summary>
        public bool IsDefault
        {
            get => isDefault;
            set => SetProperty(ref isDefault, value);
        }
        #endregion

        #region Включено
        private bool isEnabled = true;
        /// <summary>
        /// Включено
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
        #endregion

        #region Группа
        private string group = "";
        /// <summary>
        /// Группа
        /// </summary>
        public string Group
        {
            get => group;
            set => SetProperty(ref group, value);
        }
        #endregion

        #region Стиль границ
        private Thickness thickness = new(0);
        /// <summary>
        /// Стиль границ
        /// </summary>
        public Thickness BorderThickness
        {
            get => thickness;
            set => SetProperty(ref thickness, value);
        }
        #endregion

        #region Цвет заднего фона
        private IBrush background = Brushes.Transparent;
        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        public IBrush Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }
        #endregion

        #region Цвет границы
        private IBrush borderBrush = Brushes.Transparent;
        /// <summary>
        /// Цвет границы
        /// </summary>
        public IBrush BorderBrush
        {
            get => borderBrush;
            set => SetProperty(ref borderBrush, value);
        }
        #endregion

        #region Радиус скругления границ
        private CornerRadius cornerRadius = new(0);
        /// <summary>
        /// Радиус скругления границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => cornerRadius;
            set => SetProperty(ref cornerRadius, value);
        }
        #endregion
    }
}
