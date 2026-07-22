using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.SourceTable.Base
{
    public class TableCellStyle : ObservableObject
    {
        #region Цвет заднего фона
        private IBrush background;
        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        public IBrush Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }
        #endregion

        #region Цвет переднего фона
        private IBrush foreground;
        /// <summary>
        /// Цвет переднего фона
        /// </summary>
        public IBrush Foreground
        {
            get => foreground;
            set => SetProperty(ref foreground, value);
        }
        #endregion

        #region Цвет границ
        private IBrush borderBrush;
        /// <summary>
        /// Цвет границ
        /// </summary>
        public IBrush BorderBrush
        {
            get => borderBrush;
            set => SetProperty(ref borderBrush, value);
        }
        #endregion

        #region Шрифт
        private FontFamily fontFamily;
        /// <summary>
        /// Шрифт
        /// </summary>
        public FontFamily FontFamily
        {
            get => fontFamily;
            set => SetProperty(ref fontFamily, value);
        }
        #endregion

        #region Размер шрифта
        private double fontSize;
        /// <summary>
        /// Размер шрифта
        /// </summary>
        public double FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }
        #endregion

        #region Толщина шрифта
        private FontWeight currentFontWeight;
        /// <summary>
        /// Толщина шрифта
        /// </summary>
        public FontWeight CurrentFontWeight
        {
            get => currentFontWeight;
            set => SetProperty(ref currentFontWeight, value);
        }
        #endregion

        #region Стиль шрифта
        private FontStyle currentFontStyle;
        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public FontStyle CurrentFontStyle
        {
            get => currentFontStyle;
            set => SetProperty(ref currentFontStyle, value);
        }
        #endregion

        #region Подчеркнутый
        private bool underline;
        /// <summary>
        /// Подчеркнутый
        /// </summary>
        public bool IsUnderline
        {
            get => underline;
            set => SetProperty(ref underline, value);
        }
        #endregion

        #region Стиль границ
        private Thickness borderThickness;
        /// <summary>
        /// Стиль границ
        /// </summary>
        public Thickness BorderThickness
        {
            get => borderThickness;
            set => SetProperty(ref borderThickness, value);
        }
        #endregion

        #region Горизонтальное положение контента
        private HorizontalAlignment horizontalContentAlignment;
        /// <summary>
        /// Горизонтальное положение контента
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => horizontalContentAlignment;
            set => SetProperty(ref horizontalContentAlignment, value);
        }
        #endregion

        #region Вертикальное положение контента
        private VerticalAlignment verticalContentAlignment;
        /// <summary>
        /// Вертикальное положение контента
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => verticalContentAlignment;
            set => SetProperty(ref verticalContentAlignment, value);
        }
        #endregion

        #region Переносить текст
        private bool wrap;
        /// <summary>
        /// Переносить текст
        /// </summary>
        public bool IsWrap
        {
            get => wrap;
            set => SetProperty(ref wrap, value);
        }
        #endregion
    }
}
