using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.SourceTable.Base;

namespace AvaloniaTemplate.Models.SourceTable.Model
{
    public class ModelColumn : ModelBase<ModelColumn>
    {
        private readonly string DefaultBackground;
        private readonly string DefaultBackgroundSecond;
        private readonly string DefaultForeground;
        private static readonly string OnHoverForeground = Brushes.White.ToString();
        private static readonly string OnHoverBackground = new SolidColorBrush(Color.FromRgb(170, 110, 110)).ToString();
        private static readonly string OnHoverBackgroundSecond = Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString();

        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelColumn()
        {
            Owner = this;
            DefaultBackground = string.IsNullOrWhiteSpace(CellStyle?.Background) ? Brushes.Gray.ToString() : CellStyle?.Background;
            DefaultBackgroundSecond = Helper.GetAutoHighlight(Color.Parse(DefaultBackground.ToString())).ToString();
            DefaultForeground = string.IsNullOrWhiteSpace(CellStyle?.Foreground) ? Brushes.LightGray.ToString() : CellStyle?.Foreground;
        }
        #endregion

        #region Индекс
        private int index;
        /// <summary>
        /// Индекс
        /// </summary>
        public int Index
        {
            get => index;
            set => SetProperty(ref index, value);
        }
        #endregion

        #region Заголовок
        private string header;
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }
        #endregion

        #region Выбрана
        private bool selected;
        /// <summary>
        /// Выбрана
        /// </summary>
        public override bool IsSelected
        {
            get => selected;
            set => SetProperty(selected, value, x => { selected = x; ControlCurrentBackground(); });
        }
        #endregion

        #region Активная
        private bool focused;
        /// <summary>
        /// Активная
        /// </summary>
        public override bool IsFocused
        {
            get => focused;
            set => SetProperty(focused, value, x => { focused = x; ControlCurrentBackground(); });
        }
        #endregion

        #region Выбрана как заголовок
        private bool isHeader;
        /// <summary>
        /// Выбрана как заголовок
        /// </summary>
        public override bool IsHeader
        {
            get => isHeader;
            set => SetProperty(isHeader, value, x => { isHeader = x; ControlCurrentBackground(); });
        }
        #endregion

        #region Контроль текущего заднего фона
        /// <summary>
        /// Контроль текузего заднего фона
        /// </summary>
        private void ControlCurrentBackground()
        {
            CellStyle.Foreground = IsFocused || IsSelected
                ? OnHoverForeground
                : DefaultForeground;

            CellStyle.Background = IsFocused
                ? (IsHeader ? OnHoverBackgroundSecond : OnHoverBackground)
                : (IsHeader ? OnHoverBackground : (IsSelected ? DefaultBackgroundSecond : DefaultBackground));

            //if (IsFocused)
            //{
            //    if (!IsSelected)
            //    {
            //        CellStyle.Background = OnHoverBackground.ToString();
            //    }
            //    else
            //    {
            //        CellStyle.Background = IsHeader
            //            ? CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString()
            //            : CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(DefaultForeground.ToString()), 0.05).ToString();
            //    }
            //    CellStyle.Foreground = Brushes.White.ToString();
            //}
            //else
            //{
            //    CellStyle.Foreground = IsSelected
            //        ? Brushes.White.ToString()
            //        : DefaultForeground.ToString();

            //    if (!IsSelected)
            //    {
            //        CellStyle.Background = DefaultBackground.ToString();
            //    }
            //    else
            //    {
            //        CellStyle.Background = IsHeader
            //            ? CellStyle.Background = OnHoverBackground.ToString()
            //            : Helper.GetAutoHighlight(Color.Parse(DefaultBackground.ToString()), 0.05).ToString();
            //    }
            //}


            //if (IsFocused)
            //{
            //    if (IsSelected)
            //    {
            //        if (IsHeader)
            //            CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString();
            //        else
            //            CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(DefaultForeground.ToString()), 0.05).ToString();
            //    }

            //    else
            //        CellStyle.Background = OnHoverBackground.ToString();

            //    CellStyle.Foreground = Brushes.White.ToString();
            //}
            //else
            //{
            //    if (IsSelected)
            //    {
            //        if (IsHeader)
            //            CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(DefaultBackground.ToString()), 0.05).ToString();
            //        else
            //            CellStyle.Background = OnHoverBackground.ToString();

            //        CellStyle.Foreground = Brushes.White.ToString();
            //    }

            //    else
            //    {
            //        CellStyle.Foreground = DefaultForeground.ToString();
            //        CellStyle.Background = DefaultBackground.ToString();
            //    }
            //}
        }
        #endregion

        #region Сбросить статусы
        /// <summary>
        /// Сбросить статусы
        /// </summary>
        public void ResetStatus()
        {
            IsSelected = false;
            IsFocused = false;
            IsHeader = false;
        } 
        #endregion
    }
}