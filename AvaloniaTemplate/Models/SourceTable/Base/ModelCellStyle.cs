using AvaloniaTemplate.Models.SourceTable.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.SourceTable.Base
{
    public class ModelCellStyle : ObservableObject, IModelCellStyle
    {
        #region Цвет заднего фона
        private string background;
        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        public string Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }
        #endregion

        #region Цвет переднего фона
        private string foreground;
        /// <summary>
        /// Цвет переднего фона
        /// </summary>
        public string Foreground
        {
            get => foreground;
            set => SetProperty(ref foreground, value);
        }
        #endregion

        #region Цвет границ
        private string borderBrush;
        /// <summary>
        /// Цвет границ
        /// </summary>
        public string BorderBrush
        {
            get => borderBrush;
            set => SetProperty(ref borderBrush, value);
        }
        #endregion

        #region Шрифт
        private string fontFamily;
        /// <summary>
        /// Шрифт
        /// </summary>
        public string FontFamily
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

        #region Полужирный
        private bool bold;
        /// <summary>
        /// Полужирный
        /// </summary>
        public bool IsBold
        {
            get => bold;
            set => SetProperty(ref bold, value);
        }
        #endregion

        #region Курсивный
        private bool italic;
        /// <summary>
        /// Курсивный
        /// </summary>
        public bool IsItalic
        {
            get => italic;
            set => SetProperty(ref italic, value);
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

        #region Стиль нижний границы
        private string borderBottomStyle;
        /// <summary>
        /// Стиль нижний границы
        /// </summary>
        public string BorderBottomStyle
        {
            get => borderBottomStyle;
            set => SetProperty(ref borderBottomStyle, value);
        }
        #endregion

        #region Стиль верхней границы
        private string borderTopStyle;
        /// <summary>
        /// Стиль верхней границы
        /// </summary>
        public string BorderTopStyle
        {
            get => borderTopStyle;
            set => SetProperty(ref borderTopStyle, value);
        }
        #endregion

        #region Стиль левой границы
        private string borderLeftStyle;
        /// <summary>
        /// Стиль левой границы
        /// </summary>
        public string BorderLeftStyle
        {
            get => borderLeftStyle;
            set => SetProperty(ref borderLeftStyle, value);
        }
        #endregion

        #region Стиль правой границы
        private string borderRightStyle;
        /// <summary>
        /// Стиль правой границы
        /// </summary>
        public string BorderRightStyle
        {
            get => borderRightStyle;
            set => SetProperty(ref borderRightStyle, value);
        }
        #endregion

        #region Горизонтальное положение контента
        private string horizontalContentAlignment;
        /// <summary>
        /// Горизонтальное положение контента
        /// </summary>
        public string HorizontalContentAlignment
        {
            get => horizontalContentAlignment;
            set => SetProperty(ref horizontalContentAlignment, value);
        }
        #endregion

        #region Вертикальное положение контента
        private string verticalContentAlignment;
        /// <summary>
        /// Вертикальное положение контента
        /// </summary>
        public string VerticalContentAlignment
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