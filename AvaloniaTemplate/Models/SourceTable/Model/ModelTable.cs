using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaTemplate.Models.SourceTable.Model
{
    public class ModelTable : ObservableObject
    {
        private StartIndex CurrentIndex { get; } = new();
        private SelectionService<ModelColumn> SelectorColumns { get; }
        private SelectionService<ModelRow> SelectorRows { get; }
        private SelectionService<ModelCell> SelectorCells { get; }

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

        #region Событие обновления слоя DragArea
        /// <summary>
        /// Событие обновления слоя DragArea
        /// </summary>
        public Action<Orientation, Rect?, Rect?> DragAreaChangeEvent;
        #endregion

        #region Событие обновления слоя ActiveArea
        /// <summary>
        /// Событие обновления слоя ActiveArea
        /// </summary>
        public Action<List<Rect?>?> ActiveAreaChangeEvent;
        #endregion

        #region Событие обновления слоя AnchorAre
        /// <summary>
        /// Событие обновления слоя AnchorAre
        /// </summary>
        public Action<Rect?> SelectedAreaChangeEvent;
        #endregion

        #region Событие начала редактирования ячейки
        /// <summary>
        /// Событие начала редактирования ячейки
        /// </summary>
        public Action<AppActiveModeType> EditChangeEvent;
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

            SelectorCells = new SelectionService<ModelCell>(() => Cells, _selectorCell: x => (x.ColumnIndex, x.RowIndex));
            SelectorCells.SelectedChangedItem += OnSelectedCellChanged;
            SelectorCells.SelectedChangedItems += OnSelectedCellsChanged;
            SelectorCells.MultiSelectedChangedItem += OnMultiSelectedCellChanged;
            SelectorCells.SelectedAreaChanged += OnSelectedCellAreaChanged;
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
            set => SetProperty(rows, value, x =>
            {
                rows = x;
                foreach (var row in rows)
                    Cells.AddRange(row.Cells);
            });
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

        #region Коллекция ячеек
        private List<ModelCell> cells = [];
        /// <summary>
        /// Коллекция ячеек
        /// </summary>
        public List<ModelCell> Cells
        {
            get => cells;
            set => SetProperty(ref cells, value);
        }
        #endregion

        #region Коллекция видимых ячеек
        private List<ModelCell> cellsVisible = [];
        /// <summary>
        /// Коллекция видимых ячеек
        /// </summary>
        public List<ModelCell> CellsVisible
        {
            get => cellsVisible;
            set => SetProperty(ref cellsVisible, value);
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
        /// Обновить размеры текущего элемента TODO:
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

        /// <summary>
        /// Установить выбранный элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelected(PointerPressedEventArgs e, ModelCell item)
        {
            if (!e.Properties.IsLeftButtonPressed || item is not { })
                return;

            CurrentIndex.IsEqaulRow(item.RowIndex);
            CurrentIndex.IsEqaulColumn(item.ColumnIndex);
            CurrentIndex.Row = item.RowIndex;
            CurrentIndex.Column = item.ColumnIndex;

            SelectorCells.SetSelected(e, item);
        }

        /// <summary>
        /// Установить выбранный элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelected(ModelCell item)
        {
            CurrentIndex.IsEqaulRow(item.RowIndex);
            CurrentIndex.IsEqaulColumn(item.ColumnIndex);
            CurrentIndex.Row = item.RowIndex;
            CurrentIndex.Column = item.ColumnIndex;

            SelectorCells.SetSelected(item);
        }
        #endregion

        #region Установить следующую выбранную ячейку
        /// <summary>
        /// Установить следующую выбранную ячейку
        /// </summary>
        public void SelectNextCell(NavigationNextType type)
        {
            ModelCell result = null;
            if (SelectedModelCell is not { })
            {
                result = Rows[0].Cells[0];
            }
            else
            {
                switch (type)
                {
                    case NavigationNextType.Right:
                        var nextIndex = SelectedModelCell.ColumnIndex + 1;
                        if (nextIndex < 0 || nextIndex >= Columns.Count)
                            nextIndex = 0;

                        var nextCell = Rows[SelectedModelCell.RowIndex].Cells[nextIndex];
                        result = Rows[nextCell.Owner.RowIndex].Cells[nextCell.Owner.ColumnIndex];
                        break;

                    case NavigationNextType.Left:
                        nextIndex = SelectedModelCell.ColumnIndex - 1;
                        if (nextIndex < 0 || nextIndex >= Columns.Count)
                            nextIndex = 0;

                        nextCell = Rows[SelectedModelCell.RowIndex].Cells[nextIndex];
                        result = Rows[nextCell.Owner.RowIndex].Cells[nextCell.Owner.ColumnIndex];
                        break;

                    case NavigationNextType.Bottom:
                        nextIndex = SelectedModelCell.RowIndex + 1;
                        if (nextIndex < 0 || nextIndex >= Rows.Count)
                            nextIndex = 0;

                        nextCell = Rows[nextIndex].Cells[SelectedModelCell.ColumnIndex];
                        result = Rows[nextCell.Owner.RowIndex].Cells[nextCell.Owner.ColumnIndex];
                        break;

                    case NavigationNextType.Top:
                        nextIndex = SelectedModelCell.RowIndex - 1;
                        if (nextIndex < 0 || nextIndex >= Rows.Count)
                            nextIndex = 0;

                        nextCell = Rows[nextIndex].Cells[SelectedModelCell.ColumnIndex];
                        result = Rows[nextCell.Owner.RowIndex].Cells[nextCell.Owner.ColumnIndex];
                        break;
                }
            }
            if (result is not { })
                return;

            CurrentIndex.IsEqaulRow(result.RowIndex);
            CurrentIndex.IsEqaulColumn(result.ColumnIndex);
            CurrentIndex.Row = result.RowIndex;
            CurrentIndex.Column = result.ColumnIndex;
            SelectorCells.SetSelected(result);
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

        /// <summary>
        /// Событие перемещения указателя по заголовкам
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void CellsPointerMovedEvent(Control? s, PointerEventArgs e)
        {
            var point = e.GetPosition(s);
            if ((CurrentIndex.IsEqaulRow(GetRow(point.Y)) && CurrentIndex.IsEqaulColumn(GetColumn(point.X)))
                || CurrentIndex.CurrentRow < 0
                || CurrentIndex.CurrentColumn < 0
                || CurrentIndex.CurrentRow >= Rows.Count
                || CurrentIndex.CurrentColumn >= Columns.Count
                )
                return;

            SelectorCells.SetRangeSelected(CurrentIndex.CurrentColumn, CurrentIndex.CurrentRow);
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

        #region Удалить элемент
        /// <summary>
        /// Удалить элемент
        /// </summary>
        private void RemoveItem(ModelColumn item)
        {
            item.IsHeader = false;
            item.IsSelected = false;
            SelectedModelColumns.Remove(item);
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        private void RemoveItem(ModelRow item)
        {
            item.IsHeader = false;
            item.IsSelected = false;
            SelectedModelRows.Remove(item);
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        private void RemoveItem(ModelCell item)
        {
            item.IsHeader = false;
            item.IsSelected = false;
            SelectedModelCells.Remove(item);
        }
        #endregion

        #region Добавить элемент
        /// <summary>
        /// Добавить элемент
        /// </summary>
        private void AddItem(ModelColumn item)
        {
            item.IsHeader = true;
            item.IsSelected = true;
            SelectedModelColumns.Add(item);
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        private void AddItem(ModelRow item)
        {
            item.IsHeader = true;
            item.IsSelected = true;
            SelectedModelRows.Add(item);
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        private void AddItem(ModelCell item)
        {
            item.IsHeader = true;
            item.IsSelected = true;
            SelectedModelCells.Add(item);
        }
        #endregion



        #region Событие изменения текущей колонки
        /// <summary>
        /// Событие изменения текущей колонки
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedColumnChanged(ModelColumn item)
        {
            ClearSelectedModelColumns();
            SelectedModelColumn = item;

            ClearSelectedModelRows();
            SelectedModelRow = Rows[0];
            SelectedModelRows = [.. Rows];
            UpdateSelectedModelRows();

            ClearSelectedModelCells();
            SelectedModelCell = Rows[0].Cells[SelectedModelColumn.Index];
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
                    RemoveItem(item);
                    foreach (var cell in Cells.Where(x => x.ColumnIndex == item.Index))
                        RemoveItem(cell);
                }
            }

            if (added is { } && added.Any())
            {
                foreach (var item in added)
                {
                    AddItem(item);
                    foreach (var cell in Cells.Where(x => x.ColumnIndex == item.Index))
                        if (!SelectedModelCells.Contains(cell))
                            AddItem(cell);
                }
            }
            UpdateSelectedModelColumns();
            UpdateSelectedModelCells();
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
            {
                if (SelectedModelRows?.Count != Rows.Count)
                {
                    SelectedModelRows = [.. Rows];
                    UpdateSelectedModelRows();
                }
                SelectedModelColumn = item;
            }
            else
            {
                while (SelectedModelColumns?.Contains(item) == true)
                    SelectedModelColumns.Remove(item);

                if (SelectedModelColumn?.Equals(item) == true)
                    SelectedModelColumn = SelectedModelColumns?.LastOrDefault();
            }
            SelectedModelColumn ??= item;
            SelectedModelCell = Rows[0].Cells[SelectedModelColumn.Index];
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
            ActiveAreaChangeEvent?.Invoke(SelectedModelCells?.Where(x => !x.Equals(SelectedModelCell))?.Select(x => x.Geometry?.Bounds)?.ToList());

            Rect? rect = new(arg1.Value.X, 0, arg1.Value.Width, Height);
            SelectedAreaChangeEvent?.Invoke(rect);
        }
        #endregion

        #region Сбросить выбранные колонок
        /// <summary>
        /// Сбросить выбранные колонок
        /// </summary>
        private void ClearSelectedModelColumns()
        {
            foreach (var column in SelectedModelColumns)
                column.ResetStatus();

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
            SelectedModelRow = item;

            ClearSelectedModelColumns();
            SelectedModelColumn = Columns[0];
            SelectedModelColumns = [.. Columns];
            UpdateSelectedModelColumns();

            ClearSelectedModelCells();
            SelectedModelCell = item.Cells[0];
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
                    RemoveItem(item);
                    foreach (var cell in Cells.Where(x => x.RowIndex == item.Index))
                        RemoveItem(cell);
                }
            }

            if (added is { } && added.Any())
            {
                foreach (var item in added)
                {
                    AddItem(item);
                    foreach (var cell in Cells.Where(x => x.RowIndex == item.Index))
                        if (!SelectedModelCells.Contains(cell))
                            AddItem(cell);
                }
            }
            UpdateSelectedModelRows();
            UpdateSelectedModelCells();
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
            {
                if (SelectedModelColumns.Count != Columns.Count)
                {
                    SelectedModelColumns = [.. Columns];
                    UpdateSelectedModelColumns();
                }
                SelectedModelRow = item;
            }
            else
            {
                while (SelectedModelRows.Contains(item))
                    SelectedModelRows.Remove(item);

                if (SelectedModelRow.Equals(item))
                    SelectedModelRow = SelectedModelRows?.LastOrDefault();
            }
            SelectedModelRow ??= item;
            SelectedModelCell = SelectedModelRow.Cells[0];
        }
        #endregion

        #region Событие изменения выделенной области строк
        /// <summary>
        /// Событие изменения выделенной области строк
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="delete"></param>
        private void OnSelectedRowAreaChanged(Rect? arg1, bool delete)
        {
            ActiveAreaChangeEvent?.Invoke(SelectedModelCells?.Where(x => !x.Equals(SelectedModelCell))?.Select(x => x.Geometry?.Bounds)?.ToList());

            Rect? rect = new(0, arg1.Value.Y, Width, arg1.Value.Height);
            SelectedAreaChangeEvent?.Invoke(rect);
        }
        #endregion

        #region Сбросить выбранные строки
        /// <summary>
        /// Сбросить выбранные строки
        /// </summary>
        private void ClearSelectedModelRows()
        {
            foreach (var row in SelectedModelRows)
                row.ResetStatus();

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





        #region Событие изменения текущей ячейки
        /// <summary>
        /// Событие изменения текущей ячейки
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedCellChanged(ModelCell item)
        {
            ClearSelectedModelColumns();
            ClearSelectedModelRows();
            ClearSelectedModelCells();
            SelectedModelCell = item;
        }
        #endregion

        #region Событие изменения выбранных строк
        /// <summary>
        /// Событие изменения выбранных строк
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private void OnSelectedCellsChanged(IEnumerable<ModelCell> added, IEnumerable<ModelCell> removed)
        {
            if (removed is { } && removed.Any())
                foreach (var item in removed)
                    RemoveItem(item);

            if (added is { } && added.Any())
                foreach (var item in added)
                    AddItem(item);

            for (int i = SelectedModelColumns.Count - 1; i >= 0; i--)
                if (SelectedModelCells?.FirstOrDefault(x => x.ColumnIndex == SelectedModelColumns[i].Index) is not { })
                    RemoveItem(SelectedModelColumns[i]);

            for (int i = SelectedModelRows.Count - 1; i >= 0; i--)
                if (SelectedModelCells?.FirstOrDefault(x => x.RowIndex == SelectedModelRows[i].Index) is not { })
                    RemoveItem(SelectedModelRows[i]);

            foreach (var cell in SelectedModelCells)
            {
                if (!SelectedModelRows.Contains(Rows[cell.RowIndex]))
                    SelectedModelRows.Add(Rows[cell.RowIndex]);

                if (!SelectedModelColumns.Contains(Columns[cell.ColumnIndex]))
                    SelectedModelColumns.Add(Columns[cell.ColumnIndex]);
            }

            UpdateSelectedModelCells();
            UpdateSelectedModelRows();
            UpdateSelectedModelColumns();



            //if (removed is { } && removed.Any())
            //{
            //    foreach (var item in removed)
            //    {
            //        item.IsHeader = false;
            //        SelectedModelCells.Remove(item);
            //    }
            //}

            //if (added is { } && added.Any())
            //{
            //    foreach (var item in added)
            //    {
            //        item.IsHeader = true;
            //        SelectedModelCells.Add(item);
            //    }
            //}
            //ClearSelectedModelRows();
            //ClearSelectedModelColumns();
            //SetColumnsAndRowsBySelectedCells();
            //UpdateSelectedModelRows();
            //UpdateSelectedModelColumns();
            //UpdateSelectedModelCells();
        }
        #endregion

        #region Событие изменения выбранной строки
        /// <summary>
        /// Событие изменения выбранной строки
        /// </summary>
        /// <param name="item"></param>
        private void OnMultiSelectedCellChanged(ModelCell item, bool remove)
        {
            if (!remove)
                SelectedModelCell = item;
            else
            {
                while (SelectedModelCells?.Contains(item) == true)
                    SelectedModelCells.Remove(item);

                if (SelectedModelCell?.Equals(item) == true)
                    SelectedModelCell = SelectedModelCells?.LastOrDefault();
            }
            SelectedModelCell ??= item;
        }
        #endregion

        #region Событие изменения выделенной области строк
        /// <summary>
        /// Событие изменения выделенной области строк
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="delete"></param>
        private void OnSelectedCellAreaChanged(Rect? arg1, bool delete)
        {
            ActiveAreaChangeEvent?.Invoke(SelectedModelCells?.Where(x => !x.Equals(SelectedModelCell))?.Select(x => x.Geometry?.Bounds)?.ToList());

            Rect? rect = new(arg1.Value.X, arg1.Value.Y, arg1.Value.Width, arg1.Value.Height);
            SelectedAreaChangeEvent?.Invoke(rect);
        }
        #endregion

        #region Сбросить выбранные ячейки
        /// <summary>
        /// Сбросить выбранные ячейки
        /// </summary>
        private void ClearSelectedModelCells()
        {
            foreach (var cell in SelectedModelCells)
            {
                if (SelectedModelCells.FirstOrDefault(x => x.IsEdit) is { })
                    EditChangeEvent.Invoke(AppActiveModeType.Unknown);

                cell.ResetStatus();
            }
            SelectedModelCells.Clear();
        }
        #endregion

        #region Обновить выделенные ячейки модели
        /// <summary>
        /// Обновить выделенные ячейки модели
        /// </summary>
        public void UpdateSelectedModelCells()
        {
            SelectedModelCells?.Where(x => !x.IsSelected)?
                .ToList()?
                .ForEach(x => x.IsSelected = true);

            Cells.Where(x => x.IsSelected)?
                .Except(SelectedModelCells)?
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