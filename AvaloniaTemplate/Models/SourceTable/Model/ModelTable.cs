using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using AvaloniaTemplate.Models.LayoutControls.Models;
using AvaloniaTemplate.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace AvaloniaTemplate.Models.SourceTable.Model
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
        public Action<Orientation, double, double> DragStartedChange;
        #endregion

        #region Событие завершения изменения размера колонки
        /// <summary>
        /// Событие завершения изменения размера колонки
        /// </summary>
        public Action<Orientation, ModelColumn> ColumnDragCompletedChange;
        #endregion

        #region Событие завершения изменения размера строки
        /// <summary>
        /// Событие завершения изменения размера строки
        /// </summary>
        public Action<Orientation, ModelRow> RowDragCompletedChange;
        #endregion

        #region События двойного клика по разделителю колонки
        /// <summary>
        /// События двойного клика по разделителю колонки
        /// </summary>
        public Action<ModelColumn> ColumnSplitterDoubleTappedChange;
        #endregion

        #region События двойного клика по разделителю колонки
        /// <summary>
        /// События двойного клика по разделителю колонки
        /// </summary>
        public Action<ModelRow> RowSplitterDoubleTappedChange;
        #endregion

        #region События завершения обновления позиций ячеек
        /// <summary>
        /// События завершения обновления позиций ячеек
        /// </summary>
        public Action UpdateGeometryCellsFinished;
        #endregion

        #region События завершения обновления позиций колонок
        /// <summary>
        /// События завершения обновления позиций колонок
        /// </summary>
        public Action UpdateGeometryColumnsFinished;
        #endregion

        #region События завершения обновления позиций строк
        /// <summary>
        /// События завершения обновления позиций строк
        /// </summary>
        public Action UpdateGeometryRowsFinished;
        #endregion


        #region События обновления слоя DragArea
        /// <summary>
        /// События обновления слоя DragArea
        /// </summary>
        public Action<LayoutModelFrameDragArea> DragAreaEvenChange;
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

            SelectorRows = new SelectionService<ModelRow>(() => Rows, x => x.Index);
            SelectorRows.SelectedChangedItem += OnSelectedRowChanged;
            SelectorRows.SelectedChangedItems += OnSelectedRowsChanged;
            SelectorRows.MultiSelectedChangedItem += OnMultiSelectedRowChanged;
            SelectorRows.SelectedAreaChanged += OnSelectedRowAreaChanged;
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

        #region Коллекция видимых колонок
        private ObservableCollection<ModelColumn> columnsVisible = [];
        /// <summary>
        /// Коллекция видимых колонок
        /// </summary>
        public ObservableCollection<ModelColumn> ColumnsVisible
        {
            get => columnsVisible;
            set => SetProperty(ref columnsVisible, value);
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

        #region Коллекция видимых строк
        private ObservableCollection<ModelRow> rowsVisible = [];
        /// <summary>
        /// Коллекция видимых строк
        /// </summary>
        public ObservableCollection<ModelRow> RowsVisible
        {
            get => rowsVisible;
            set => SetProperty(ref rowsVisible, value);
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

        

        

        #region Обновить размеры текущего элемента
        /// <summary>
        /// Обновить размеры текущего элемента
        /// </summary>
        /// <param name="column"></param>
        /// <param name="delta"></param>
        public void Resize(ModelColumn column, double delta)
        {
            column.Geometry.Width += delta;
            var posX = column.Geometry.Right;
            Columns.Where(x => x.Index > column.Index)?
                .ToList()?
                .ForEach(x =>
                {
                    x.Geometry.PositionX = posX;
                    posX = x.Geometry.Right;
                });
        }

        /// <summary>
        /// Обновить размеры текущего элемента
        /// </summary>
        /// <param name="column"></param>
        /// <param name="delta"></param>
        public void Resize(ModelRow row, double delta)
        {
            row.Geometry.Height += delta;
            var posY = row.Geometry.Bottom;
            Rows.Where(x => x.Index > row.Index)?
                .ToList()?
                .ForEach(x =>
                {
                    x.Geometry.PositionY = posY;
                    posY = x.Geometry.Bottom;
                });
        }
        #endregion

        #region Обновить позиционирование ячеек
        /// <summary>
        /// Обновить позиционирование ячеек
        /// </summary>
        public void UpdateGeometryCells()
        {
            foreach (var row in Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.Geometry.Width = Columns[cell.ColumnIndex].Geometry.Width;
                    cell.Geometry.PositionX = Columns[cell.ColumnIndex].Geometry.PositionX;
                    cell.Geometry.Height = row.Geometry.Height;
                    cell.Geometry.PositionY = row.Geometry.PositionY;
                }
            }
            UpdateGeometryCellsFinished?.Invoke();

        } 
        #endregion

        #region Установить выбранный элемент
        /// <summary>
        /// Установить выбранный элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelected(PointerPressedEventArgs e, ModelColumn item)
        {
            if (!e.Properties.IsLeftButtonPressed || item is not { })
                return;

            CurrentIndex.IsEqaulColumn(item.Index);
            CurrentIndex.Column = item.Index;
            SelectorColumns.SetSelected(e, item);
        }

        /// <summary>
        /// Установить выбранный элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelected(PointerPressedEventArgs e, ModelRow item)
        {
            if (!e.Properties.IsLeftButtonPressed || item is not { })
                return;

            CurrentIndex.IsEqaulRow(item.Index);
            CurrentIndex.Row = item.Index;
            SelectorRows.SetSelected(e, item);
        }
        #endregion

        #region Событие перемещения указателя по заголовкам
        /// <summary>
        /// Событие перемещения указателя по заголовкам
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

        /// <summary>
        /// Событие перемещения указателя по заголовкам
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void RowsPointerMovedEvent(Control? s, PointerEventArgs e)
        {
            var point = e.GetPosition(s);
            if (CurrentIndex.IsEqaulRow(GetRow(point.Y))
                || CurrentIndex.CurrentRow < 0
                || CurrentIndex.CurrentRow >= Rows.Count
                )
                return;

            SelectorRows.SetRangeSelected(CurrentIndex.CurrentRow);
        }
        #endregion

        #region Установить фокус
        /// <summary>
        /// Установить фокус
        /// </summary>
        /// <param name="item"></param>
        public void SetFocus(ModelColumn item)
            => SelectorColumns.SetFocus(item);

        /// <summary>
        /// Установить фокус
        /// </summary>
        /// <param name="item"></param>
        public void SetFocus(ModelRow item)
            => SelectorRows.SetFocus(item);
        #endregion

        #region Снять фокус
        /// <summary>
        /// Снять фокус
        /// </summary>
        /// <param name="item"></param>
        public void ResetFocus(ModelColumn item)
            => SelectorColumns.ResetFocus(item);

        /// <summary>
        /// Снять фокус
        /// </summary>
        /// <param name="item"></param>
        public void ResetFocus(ModelRow item)
            => SelectorRows.ResetFocus(item);
        #endregion


        #region Событие изменения текущей колонки
        /// <summary>
        /// Событие изменения текущей колонки
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedColumnChanged(ModelColumn item)
        {
            ClearSelectedModelColumns();
            SelectedModelColumns.Add(item);
            SelectedModelColumn = item;
            UpdateSelectedModelColumns();

            ClearSelectedModelRows();
            SelectedModelRows = [.. Rows];
            SelectedModelRow = Rows[0];
            UpdateSelectedModelRows();


            //SelectedModelRows.Clear();
            //SelectedModelRows = [.. Rows];
            //SelectedModelRow = Rows[0];
            //SelectedModelRow.IsHeader = false;

            //SelectedModelColumns.Clear();
            //SelectedModelColumns.Add(item);
            //SelectedModelColumn = item;
            ////UpdateSelectedModelColumns(true);

            //SelectedModelRows.Clear();
            //SelectedModelRows = [.. Rows];
            //SelectedModelRow = Rows[0];
            //SelectedModelRow.IsHeader = true;
            //UpdateSelectedModelRows();

            //SelectedModelCells.Clear();
            //foreach (var row in SelectedModelRows)
            //    SelectedModelCells.Add(row.Cells[item.Index]);

            //SelectedModelCell = SelectedModelRows[0].Cells[0];
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
            {
                foreach (var item in removed)
                {
                    item.IsHeader = false;
                    SelectedModelColumns.Remove(item);
                }
            }

            if (added is { } && added.Any())
            {
                foreach (var item in added)
                {
                    item.IsHeader = true;
                    SelectedModelColumns.Add(item);
                }
            }
            UpdateSelectedModelColumns();
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

        #region Сбросить выбранные колонок
        /// <summary>
        /// Сбросить выбранные колонок
        /// </summary>
        private void ClearSelectedModelColumns()
        {
            foreach (var item in SelectedModelColumns)
            {
                item.IsSelected = false;
                item.IsFocused = false;
                item.IsHeader = false;
            }
            SelectedModelColumns.Clear();
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

                if (posX < col.Geometry.PositionX)
                    right = mid - 1;
                else if (posX > col.Geometry.Right)
                    left = mid + 1;
                else
                    return mid;
            }
            return -1;
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
                .ToList()?
                .ForEach(x =>
                {
                    x.IsHeader = false;
                    x.IsSelected = false;
                });
        }
        #endregion





        #region Событие изменения текущей строки
        /// <summary>
        /// Событие изменения текущей строки
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedRowChanged(ModelRow item)
        {
            ClearSelectedModelRows();
            SelectedModelRows.Add(item);
            SelectedModelRow = item;
            UpdateSelectedModelRows();

            ClearSelectedModelColumns();
            SelectedModelColumns = [.. Columns];
            SelectedModelColumn = Columns[0];
            UpdateSelectedModelColumns();





            //ClearSelectedModelColumns();
            //SelectedModelColumns.Add(item);
            //SelectedModelColumn = item;
            //SelectedModelColumn.IsHeader = true;
            //UpdateSelectedModelColumns();

            //ClearSelectedModelRows();
            //SelectedModelRows = [.. Rows];
            //SelectedModelRow = Rows[0];
            //SelectedModelRow.IsHeader = false;
            //UpdateSelectedModelRows();



            //SelectedModelColumns.Clear();
            //SelectedModelColumns = [.. Columns];
            //SelectedModelColumn = Columns[0];
            //SelectedModelColumn.IsHeader = false;
            //UpdateSelectedModelColumns();

            //SelectedModelRows.Clear();
            //SelectedModelRows.Add(item);
            //SelectedModelRow = item;
            ////UpdateSelectedModelRows(true);

            //SelectedModelCells.Clear();
            //SelectedModelCells = [.. item.Cells];
            //SelectedModelCell = item.Cells[0];
        }
        #endregion

        #region Событие изменения выбранных строк
        /// <summary>
        /// Событие изменения выбранных строк
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private void OnSelectedRowsChanged(IEnumerable<ModelRow> added, IEnumerable<ModelRow> removed)
        {
            if (removed is { } && removed.Any())
            {
                foreach (var item in removed)
                {
                    item.IsHeader = false;
                    SelectedModelRows.Remove(item);
                }
            }

            if (added is { } && added.Any())
            {
                foreach (var item in added)
                {
                    item.IsHeader = true;
                    SelectedModelRows.Add(item);
                }
            }
            UpdateSelectedModelRows();
        }
        #endregion

        #region Событие изменения выбранной строки
        /// <summary>
        /// Событие изменения выбранной строки
        /// </summary>
        /// <param name="item"></param>
        private void OnMultiSelectedRowChanged(ModelRow item, bool remove)
        {
            if (!remove)
                SelectedModelRow = item;
            else
            {
                while (SelectedModelRows.Contains(item))
                    SelectedModelRows.Remove(item);

                if (SelectedModelRow.Equals(item))
                    SelectedModelRow = SelectedModelRows?.LastOrDefault();
            }
        }
        #endregion

        #region Событие изменения выделенной области колонок
        /// <summary>
        /// Событие изменения выделенной области колонок
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="delete"></param>
        private void OnSelectedRowAreaChanged(Rect? arg1, bool delete)
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

        #region Сбросить выбранные строки
        /// <summary>
        /// Сбросить выбранные строки
        /// </summary>
        private void ClearSelectedModelRows()
        {
            foreach (var item in SelectedModelRows)
            {
                item.IsSelected = false;
                item.IsFocused = false;
                item.IsHeader = false;
            }
            SelectedModelRows.Clear();
        }
        #endregion

        #region Получить индекс колонки
        /// <summary>
        /// Получить индекс колонки
        /// </summary>
        /// <returns></returns>
        private int GetRow(double posY)
        {
            int top = 0;
            int bottom = Rows.Count - 1;

            while (top <= bottom)
            {
                int mid = (top + bottom) / 2;
                var row = Rows[mid];

                if (posY < row.Geometry.PositionY)
                    bottom = mid - 1;
                else if (posY > row.Geometry.Bottom)
                    top = mid + 1;
                else
                    return mid;
            }
            return -1;
        }
        #endregion

        #region Обновить выделенные строк модели
        /// <summary>
        /// Обновить выделенные строк модели
        /// </summary>
        public void UpdateSelectedModelRows()
        {
            SelectedModelRows?.Where(x => !x.IsSelected)?
                .ToList()?
                .ForEach(x => x.IsSelected = true);

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
    }
}