using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Table.Base;

namespace AvaloniaTemplate.Models.Table.Model
{
    public class ModelColumn : ModelBase<ModelColumn>
    {
        private readonly IBrush DefaultBackground;
        private readonly IBrush DefaultForeground;
        private readonly IBrush OnHoverBackground = new SolidColorBrush(Color.FromRgb(170, 110, 110));

        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelColumn()
        {
            Owner = this;
            DefaultBackground = Helper.GetColor(CellStyle?.Background ?? Brushes.Gray.ToString());
            DefaultForeground = Helper.GetColor(CellStyle?.Foreground ?? Brushes.LightGray.ToString());
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

        #region Ширина
        private double widthResult;
        /// <summary>
        /// Ширина
        /// </summary>
        public double WidthResult
        {
            get => widthResult;
            set => SetProperty(ref widthResult, value);
        }
        #endregion

        #region Контроль текущего заднего фона
        /// <summary>
        /// Контроль текузего заднего фона
        /// </summary>
        private void ControlCurrentBackground()
        {
            if (IsFocused)
            {
                if (IsSelected)
                    CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString();
                else
                    CellStyle.Background = OnHoverBackground.ToString();

                CellStyle.Foreground = Brushes.White.ToString();
            }
            else
            {
                if (IsSelected)
                {
                    CellStyle.Foreground = Brushes.White.ToString();
                    CellStyle.Background = OnHoverBackground.ToString();
                }

                else
                {
                    CellStyle.Foreground = DefaultForeground.ToString();
                    CellStyle.Background = DefaultBackground.ToString();
                }

            }
        }
        #endregion
    }
}