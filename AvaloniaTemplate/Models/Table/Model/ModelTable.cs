using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using AvaloniaTemplate.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaTemplate.Models.Table.Model
{
    public class ModelTable : ObservableObject
    {

        private StartIndex CurrentIndex { get; } = new();
        private SelectionService<ModelColumn> SelectorColumns { get; }
        private SelectionService<ModelRow> SelectorRows { get; }

        #region Событие изменения размера колонки
        /// <summary>
        /// Событие изменения размера колонки
        /// </summary>
        public Action<Orientation, double, double> ColumnDragStartedChange;
        #endregion

        #region Событие завершения изменения размера колонки
        /// <summary>
        /// Событие завершения изменения размера колонки
        /// </summary>
        public Action<Orientation, ModelColumn> ColumnDragCompletedChange;
        #endregion

        #region События двойного клика по разделителю колонки
        /// <summary>
        /// События двойного клика по разделителю колонки
        /// </summary>
        public Action<ModelColumn> ColumnSplitterDoubleTappedChange;
        #endregion


        

        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        public ModelTable()
        {
            SelectorColumns = new SelectionService<ModelColumn>(() => Columns, x => x.Index);
            SelectorColumns.SelectedChangedItem += OnSelectedColumnChanged;
            SelectorColumns.SelectedChangedItems += OnSelectedColumnsChanged;
            SelectorColumns.MultiSelectedChangedItem += OnMultiSelectedColumnChanged;
            SelectorColumns.SelectedAreaChanged += OnSelectedColumnAreaChanged;
        }
        #endregion

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
            set => SetProperty(ref selectedModelColumn, value);
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

        #region Выбранная ячейка
        private ModelCell selectedModelCell;
        /// <summary>
        /// Выбранная ячейка
        /// </summary>
        public ModelCell SelectedModelCell
        {
            get => selectedModelCell;
            set => SetProperty(ref selectedModelCell, value);
        }
        #endregion

        #region Выбранные ячейки
        private ObservableCollection<ModelCell> selectedModelCells = [];
        /// <summary>
        /// Выбранные ячейки
        /// </summary>
        public ObservableCollection<ModelCell> SelectedModelCells
        {
            get => selectedModelCells;
            set => SetProperty(ref selectedModelCells, value);
        }
        #endregion

        #region Обновить выделенные колонки модели
        /// <summary>
        /// Обновить выделенные колонки модели
        /// </summary>
        public void UpdateSelectedModelColumns(bool isHeader)
        {
            SelectedModelColumns?.Where(x => !x.IsSelected)?
                .ToList()?
                .ForEach(x =>
                {
                    x.IsHeader = isHeader;
                    x.IsSelected = true;
                });

            Columns.Where(x => x.IsSelected)?
                .Except(SelectedModelColumns)?
                .ToList()?
                .ForEach(x =>
                {
                    x.IsHeader = false;
                    x.IsSelected = false;
                });
        }
        #endregion

        #region Обновить выделенные строк модели
        /// <summary>
        /// Обновить выделенные строк модели
        /// </summary>
        public void UpdateSelectedModelRows(bool isHeader)
        {
            SelectedModelRows?.Where(x => !x.IsSelected)?
                .ToList()?
                .ForEach(x =>
                {
                    x.IsHeader = isHeader;
                    x.IsSelected = true;
                });

            Rows.Where(x => x.IsSelected)?
                .Except(SelectedModelRows)?
                .ToList()?
                .ForEach(x =>
                {
                    x.IsHeader = false;
                    x.IsSelected = false;
                });
        }
        #endregion

        #region Установить выбранную колонку
        /// <summary>
        /// Установить выбранную колонку
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelectedColumn(PointerPressedEventArgs e, ModelColumn item)
        {
            if (!e.Properties.IsLeftButtonPressed || item is not { })
                return;

            CurrentIndex.IsEqaulColumn(item.Index);
            CurrentIndex.Column = item.Index;
            SelectorColumns.SetSelected(e, item);
        }
        #endregion

        #region Событие перемещения указателя по заголовкам колонок
        /// <summary>
        /// Событие перемещения указателя по заголовкам колонок
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void ColumnsPointerMovedEvent(Control? s, PointerEventArgs e)
        {
            var point = e.GetPosition(s);
            if (CurrentIndex.IsEqaulColumn(GetColumn(point.X))
                || CurrentIndex.CurrentColumn < 0
                || CurrentIndex.CurrentColumn >= Columns.Count
                )
                return;

            SelectorColumns.SetRangeSelected(CurrentIndex.CurrentColumn);
        }
        #endregion




        #region Установить фокус
        /// <summary>
        /// Установить фокус
        /// </summary>
        /// <param name="item"></param>
        public void SetFocusColumn(ModelColumn item)
            => SelectorColumns.SetFocus(item);
        #endregion

        #region Снять фокус
        /// <summary>
        /// Снять фокус
        /// </summary>
        /// <param name="item"></param>
        public void ResetFocusColumn(ModelColumn item)
            => SelectorColumns.ResetFocus(item);
        #endregion

        #region Событие изменения текущей колонки
        /// <summary>
        /// Событие изменения текущей колонки
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedColumnChanged(ModelColumn item)
        {
            SelectedModelColumns.Clear();
            SelectedModelColumns.Add(item);
            SelectedModelColumn = item;
            UpdateSelectedModelColumns(true);

            SelectedModelRows.Clear();
            SelectedModelRows = [.. Rows];
            SelectedModelRow = Rows[0];
            UpdateSelectedModelRows(false);

            foreach (var row in SelectedModelRows)
            {
                SelectedModelCells.Clear();
                SelectedModelCells.Add(row.Cells[item.Index]);
            }
            SelectedModelCell = SelectedModelRows[0].Cells[0];
        }
        #endregion

        #region Событие изменения выбранных колонок
        /// <summary>
        /// Событие изменения выбранных колонок
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private void OnSelectedColumnsChanged(IEnumerable<ModelColumn> added, IEnumerable<ModelColumn> removed)
        {
            if (removed is { } && removed.Any())
                foreach (var item in removed)
                    SelectedModelColumns.Remove(item);

            if (added is { } && added.Any())
                foreach (var item in added)
                    SelectedModelColumns.Add(item);

            UpdateSelectedModelColumns(true);
        }
        #endregion

        #region Событие изменения выбранной колонки
        /// <summary>
        /// Событие изменения выбранной колонки
        /// </summary>
        /// <param name="item"></param>
        private void OnMultiSelectedColumnChanged(ModelColumn item, bool remove)
        {
            if (!remove)
                SelectedModelColumn = item;
            else
            {
                while (SelectedModelColumns.Contains(item))
                    SelectedModelColumns.Remove(item);

                if (SelectedModelColumn.Equals(item))
                    SelectedModelColumn = SelectedModelColumns?.LastOrDefault();
            }
        }
        #endregion

        #region Событие изменения выделенной области колонок
        /// <summary>
        /// Событие изменения выделенной области колонок
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="delete"></param>
        private void OnSelectedColumnAreaChanged(Rect? arg1, bool delete)
        {

            //Rect? rect = new(arg1.Value.X, 0, arg1.Value.Width, TableFactory.SelectedModel.Height);
            //TableFactory?.UpdateSelectedArea(rect);


            //if (delete)
            //{
            //    var rect = new Rect(arg1.Value.X, arg1.Value.Y, arg1.Value.Width, Rows.Sum(x => x.Height.Value));
            //    MainFrame.RemoveArea ??= new()
            //    {
            //        RectPen = new Pen(new SolidColorBrush(Helper.GetAutoHighlight(Brushes.LightGray)), 2),
            //        RectFill = new SolidColorBrush(Helper.GetAutoHighlight(Brushes.LightGray), 0.1)
            //    };
            //    MainFrame.RemoveArea.Area = rect;
            //    MainFrame.InvalidateVisual();
            //}
            //else
            //{
            //    if (MainFrame?.RemoveArea?.Area is { } area)
            //    {
            //        var arrayCells = SelectedCells?
            //            .Where(x => (x.PositionX < area.X || x.PositionX >= area.Right) && (x.IsSelected || x.IsFocused))?
            //            .ToList();
            //        SelectedCells.Clear();

            //        if (arrayCells is { } && arrayCells.Count > 0)
            //            UpdateSelectedCells([.. arrayCells], []);

            //        var arrayCols = SelectedColumns?
            //            .Where(x => (x.PositionX < area.X || x.PositionX >= area.Right) && x.IsFocused)?
            //            .ToList();
            //        SelectedColumns.Clear();
            //        if (arrayCols is { } && arrayCols.Count > 0)
            //            UpdateFocusColumns([.. arrayCols], []);

            //        arrayCols = Columns?
            //            .Where(x => (x.PositionX < area.X || x.PositionX >= area.Right) && x.IsSelected)?
            //            .ToList();
            //        UpdateSelectedColumns([.. arrayCols]);

            //        UpdateSelectedAreaCells(SelectorColumns.GetIsCtrl(), SelectorCells.BuildBoundingRect([SelectedCell]));
            //        MainFrame.RemoveArea = null;
            //        MainFrame.InvalidateVisual();

            //        if (SelectedColumns.Count <= 0 && SelectedCells.Count <= 0)
            //            UpdateSelectedRows([]);
            //    }
            //}
        }
        #endregion

        #region Получить индекс колонки
        /// <summary>
        /// Получить индекс колонки
        /// </summary>
        /// <returns></returns>
        private int GetColumn(double posX)
        {
            int left = 0;
            int right = Columns.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                var col = Columns[mid];

                if (posX < col.PositionX)
                    right = mid - 1;
                else if (posX > col.PositionX + col.Width)
                    left = mid + 1;
                else
                    return mid;
            }
            return -1;
        }
        #endregion
    }
}