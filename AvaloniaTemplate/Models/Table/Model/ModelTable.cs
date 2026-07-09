using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaTemplate.Models.Table.Model
{
    public class ModelTable : ObservableObject
    {
        #region Идентификатор
        private string id;
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
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

        #region Масштаб
        private int scale;
        /// <summary>
        /// Масштаб
        /// </summary>
        public int Scale
        {
            get => scale;
            set => SetProperty(ref scale, value);
        }
        #endregion

        #region Ширина
        private double width;
        /// <summary>
        /// Ширина
        /// </summary>
        public double Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }
        #endregion

        #region Высота
        private double height;
        /// <summary>
        /// Высота
        /// </summary>
        public double Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }
        #endregion

        #region Положение по оси X
        private double positionX;
        /// <summary>
        /// Положение по оси X
        /// </summary>
        public double PositionX
        {
            get => positionX;
            set => SetProperty(ref positionX, value);
        }
        #endregion

        #region Положение по оси Y
        private double positionY;
        /// <summary>
        /// Положение по оси Y
        /// </summary>
        public double PositionY
        {
            get => positionY;
            set => SetProperty(ref positionY, value);
        }
        #endregion

        #region Видимость
        private bool visible;
        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsVisible
        {
            get => visible;
            set => SetProperty(ref visible, value);
        }
        #endregion

        #region Выбрана
        private bool selected;
        /// <summary>
        /// Выбрана
        /// </summary>
        public bool IsSelected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }
        #endregion

        #region Активная
        private bool focused;
        /// <summary>
        /// Активная
        /// </summary>
        public bool IsFocused
        {
            get => focused;
            set => SetProperty(ref focused, value);
        }
        #endregion

        #region Цвет фона
        private string background;
        /// <summary>
        /// Цвет фона
        /// </summary>
        public string Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }
        #endregion

        #region Цвет сетки
        private string frameBrush;
        /// <summary>
        /// Цвет сетки
        /// </summary>
        public string FrameBrush
        {
            get => frameBrush;
            set => SetProperty(ref frameBrush, value);
        }
        #endregion

        #region Цвет рамки
        private string borderBrush;
        /// <summary>
        /// Цвет рамки
        /// </summary>
        public string BorderBrush
        {
            get => borderBrush;
            set => SetProperty(ref borderBrush, value);
        }
        #endregion

        #region Высота заголовка колонок
        private double headerColumnsHeight;
        /// <summary>
        /// Высота заголовка колонок
        /// </summary>
        public double HeaderColumnsHeight
        {
            get => headerColumnsHeight;
            set => SetProperty(ref headerColumnsHeight, value);
        }
        #endregion

        #region Ширина заголовка строк
        private double headerRowsWidth;
        /// <summary>
        /// Ширина заголовка строк
        /// </summary>
        public double HeaderRowsWidth
        {
            get => headerRowsWidth;
            set => SetProperty(ref headerRowsWidth, value);
        }
        #endregion

        #region Коллекция колонок
        private ObservableCollection<ModelColumn> columns = [];
        /// <summary>
        /// Коллекция колонок
        /// </summary>
        public ObservableCollection<ModelColumn> Columns
        {
            get => columns;
            set => SetProperty(ref columns, value);
        }
        #endregion

        #region Выбранная колонка
        private ModelColumn selectedModelColumn;
        /// <summary>
        /// Выбранная колонка
        /// </summary>
        public ModelColumn SelectedModelColumn
        {
            get => selectedModelColumn;
            set => SetProperty(selectedModelColumn, value, x => { selectedModelColumn = x; UpdateSelectedModelColumns(); });
        }
        #endregion

        #region Выбранные колонки
        private ObservableCollection<ModelColumn> selectedModelColumns = [];
        /// <summary>
        /// Выбранные колонки
        /// </summary>
        public ObservableCollection<ModelColumn> SelectedModelColumns
        {
            get => selectedModelColumns;
            set => SetProperty(ref selectedModelColumns, value);
        }
        #endregion

        #region Коллекция строк
        private ObservableCollection<ModelRow> rows = [];
        /// <summary>
        /// Коллекция строк
        /// </summary>
        public ObservableCollection<ModelRow> Rows
        {
            get => rows;
            set => SetProperty(ref rows, value);
        }
        #endregion

        #region Выбранная строка
        private ModelRow selectedModelRow;
        /// <summary>
        /// Выбранная строка
        /// </summary>
        public ModelRow SelectedModelRow
        {
            get => selectedModelRow;
            set => SetProperty(ref selectedModelRow, value);
        }
        #endregion

        #region Выбранные строки
        private ObservableCollection<ModelRow> selectedModelRows = [];
        /// <summary>
        /// Выбранные строки
        /// </summary>
        public ObservableCollection<ModelRow> SelectedModelRows
        {
            get => selectedModelRows;
            set => SetProperty(ref selectedModelRows, value);
        }
        #endregion

        #region Обновить выделенные колонки модели
        /// <summary>
        /// Обновить выделенные колонки модели
        /// </summary>
        public void UpdateSelectedModelColumns()
        {
            SelectedModelColumns?.Where(x => !x.IsSelected)?
                .ToList()?
                .ForEach(x => x.IsSelected = true);

            Columns.Where(x => x.IsSelected)?
                .Except(SelectedModelColumns)?
                .ToList() ?
                .ForEach(x => x.IsSelected = false);

            //set
            //{
            //    if (SetProperty(ref selectedModelColumn, value))
            //        UpdateSelectedModelColumns()
            //    {
            //        SelectedModel.Columns.Where(x => x.IsSelected)?
            //            .Except(SelectedModelColumns)?
            //            .ToList()?
            //            .ForEach(x => x.IsSelected = false);

            //        SelectedModelColumns?.ToList()?
            //            .ForEach(x => x.IsSelected = true);
            //    }
            //}
        }
        #endregion
    }
}