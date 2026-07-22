using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.SourceTable.Base;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.Models.SourceTable.Model
{
    public class ModelRow : ModelBase<ModelRow>
    {
        private readonly IBrush DefaultBackground;
        private readonly IBrush DefaultForeground;
        private readonly IBrush OnHoverBackground = new SolidColorBrush(Color.FromRgb(170, 110, 110));

        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelRow()
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

        #region Коллекция ячеек
        private ObservableCollection<ModelCell> cells = [];
        /// <summary>
        /// Коллекция ячеек
        /// </summary>
        public ObservableCollection<ModelCell> Cells
        {
            get => cells;
            set => SetProperty(ref cells, value);
        }
        #endregion

        #region Коллекция видимых ячеек
        private ObservableCollection<ModelCell> cellsVisible = [];
        /// <summary>
        /// Коллекция видимых ячеек
        /// </summary>
        public ObservableCollection<ModelCell> CellsVisible
        {
            get => cellsVisible;
            set => SetProperty(ref cellsVisible, value);
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
        public bool IsHeader
        {
            get => isHeader;
            set => SetProperty(ref isHeader, value);
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
                CellStyle.Foreground = Brushes.White.ToString();
                if (!IsSelected)
                {
                    CellStyle.Background = OnHoverBackground.ToString();
                }
                else
                {
                    CellStyle.Background = IsHeader
                        ? Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString()
                        : Helper.GetAutoHighlight(Color.Parse(DefaultForeground.ToString()), 0.05).ToString();
                }
            }
            else
            {
                CellStyle.Foreground = IsSelected
                    ? Brushes.White.ToString()
                    : DefaultForeground.ToString();

                if (!IsSelected)
                {
                    CellStyle.Background = DefaultBackground.ToString();
                }
                else
                {
                    CellStyle.Background = IsHeader
                        ? OnHoverBackground.ToString()
                        : Helper.GetAutoHighlight(Color.Parse(DefaultBackground.ToString()), 0.05).ToString();
                }
            }



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

                //if (IsFocused)
                //{
                //    if (IsSelected)
                //        CellStyle.Background = Helper.GetAutoHighlight(Color.Parse(OnHoverBackground.ToString()), 0.05).ToString();
                //    else
                //        CellStyle.Background = OnHoverBackground.ToString();

                //    CellStyle.Foreground = Brushes.White.ToString();
                //}
                //else
                //{
                //    if (IsSelected)
                //    {
                //        CellStyle.Foreground = Brushes.White.ToString();
                //        CellStyle.Background = OnHoverBackground.ToString();
                //    }

                //    else
                //    {
                //        CellStyle.Foreground = DefaultForeground.ToString();
                //        CellStyle.Background = DefaultBackground.ToString();
                //    }
                //}
        }
        #endregion
    }
}