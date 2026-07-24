using Avalonia;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaTemplate.Services
{
    public class TableGenerateFactory : ObservableObject, ITableGenerateFactory
    {
        private const double ColumnWidthDefault = 60;
        private const double ColumnHeightDefault = 30;
        private const double RowWidthDefault = 30;
        private const double RowHeightDefault = 20;
        private const int ColumnCountDefault = 200;
        private const int RowCountDefault = 150;

        #region Сервис обработки состояний
        /// <summary>
        /// Сервис обработки состояний
        /// </summary>
        private IUIConnectorService ConnectorService { get; } = App.GetService<IUIConnectorService>();
        #endregion

        #region Сервис управления панелью со вкладками
        /// <summary>
        /// Сервис управления панелью со вкладками
        /// </summary>
        private IHorizontalTabStripService<ModelTable> TabStripService { get; } = App.GetService<IHorizontalTabStripService<ModelTable>>();
        #endregion

        #region Сервис управления масштабом
        /// <summary>
        /// Сервис управления масштабом
        /// </summary>
        private IZoomService ZoomService { get; } = App.GetService<IZoomService>();
        #endregion

        #region Сервис управления панелью прокрутки
        /// <summary>
        /// Сервис управления панелью прокрутки
        /// </summary>
        private IScrollBarService ScrollBarService { get; } = App.GetService<IScrollBarService>();
        #endregion

        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public TableGenerateFactory()
        {
            ZoomService.ScaleChange += OnScaleChange;
            TabStripService.CreateItem += CreateModel;
            TabStripService.SelectedItemChange += OnSelectedItemChange;
            if (Models is not { })
            {
                Models ??= [];
                TabStripService.ItemsSource = Models;
                TabStripService.Command_CreateItem.Execute(Models);
            }

            ScrollBarService.PositionChange += (x, y) =>
            {
                UpdateElementsVisible(x, y, ConnectorService.WindowWidth, ConnectorService.WindowHeight);
                SelectedModel.PositionX = x;
                SelectedModel.PositionY = y;
            };
        }
        #endregion

        #region Коллекция моделей
        private ObservableCollection<ModelTable> models;
        /// <summary>
        /// Коллекция моделей
        /// </summary>
        public ObservableCollection<ModelTable> Models
        {
            get => models;
            set => SetProperty(ref models, value);
        }
        #endregion

        #region Выбранная модель
        private ModelTable selectedModel;
        /// <summary>
        /// Выбранная модель
        /// </summary>
        public ModelTable SelectedModel
        {
            get => selectedModel;
            set => SetProperty(ref selectedModel, value);
        }
        #endregion

        #region Выбранные модели
        private ObservableCollection<ModelTable> selectedModels;
        /// <summary>
        /// Выбранные модели
        /// </summary>
        public ObservableCollection<ModelTable> SelectedModels
        {
            get => selectedModels;
            set => SetProperty(ref selectedModels, value);
        }
        #endregion

        #region Создать модель
        /// <summary>
        /// Создать модель
        /// </summary>
        public ModelTable CreateModel()
        {
            var header = CreateHeader();
            var model = new ModelTable()
            {
                Id = header,
                Index = Models.Count,
                Header = header,
                Scale = 100,
                Width = ColumnCountDefault * ColumnWidthDefault,
                Height = RowCountDefault * RowHeightDefault,
                PositionX = 0,
                PositionY = 0,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                Background = Helper.GetResource<Color>("AppBackground").ToString(),
                FrameBrush = Helper.GetColor(Brushes.LightGray),
                BorderBrush = Helper.GetColor(Brushes.Transparent),
                HeaderColumnsHeight = ColumnHeightDefault,
                HeaderRowsWidth = RowWidthDefault,
                Columns = [.. CreateModelColumns(0, ColumnCountDefault)],
                Rows = [.. CreateModelRows(0, RowCountDefault)]
            };

            model.DragStartedChange += UpdateLayoutDrag;
            model.EditChangeEvent += OnEditChangeEvent;

            model.ColumnDragCompletedChange += UpdateLayoutDragComplete;
            model.ColumnSplitterDoubleTappedChange += SetSizeColumnToContent;

            model.RowDragCompletedChange += UpdateLayoutDragComplete;
            model.RowSplitterDoubleTappedChange += SetSizeRowToContent;

            return model;
        }
        #endregion

        #region Удалить модель
        /// <summary>
        /// Удалить модель
        /// </summary>
        public void DeleteModel()
            => DeleteModel(SelectedModel);

        /// <summary>
        /// Удалить модель
        /// </summary>
        /// <param name="model"></param>
        public void DeleteModel(ModelTable model)
        {
            TabStripService.ItemsSource.Remove(model);
            Models.Remove(model);
            if (SelectedModel.Equals(model))
                SelectedModel = Helper.GetSelectedElement<ModelTable>(model.Index, Models);
        }

        /// <summary>
        /// Удалить модель
        /// </summary>
        /// <param name="models"></param>
        public void DeleteModel(ObservableCollection<ModelTable> models)
        {
            foreach (var model in models)
                DeleteModel(model);
        }
        #endregion

        #region Создать модель колонки
        /// <summary>
        /// Создать модель колонки
        /// </summary>
        /// <param name="index"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public ModelColumn CreateModelColumn(int index, string header)
        {
            return new()
            {
                Id = header,
                Index = index,
                Header = header,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                Geometry = new Models.Geometry()
                {
                    Width = ColumnWidthDefault,
                    Height = ColumnHeightDefault,
                    PositionX = index * ColumnWidthDefault,
                    PositionY = 0,
                },
                CellStyle = new()
                {
                    Background = Helper.GetColor(Brushes.Transparent),
                    Foreground = Helper.GetColor(Brushes.LightGray),
                    BorderBrush = Helper.GetColor(Brushes.LightGray),
                    FontFamily = FontFamilyHelper.AppFontDefault.Name,
                    FontSize = FontFamilyHelper.FontSizeDefault,
                    IsBold = true,
                    IsItalic = false,
                    IsUnderline = false,
                    IsWrap = false,
                    HorizontalContentAlignment = HorizontalAlignment.Center.ToString(),
                    VerticalContentAlignment = VerticalAlignment.Center.ToString(),
                    BorderBottomStyle = BorderLineStyleType.Normal.ToString(),
                    BorderTopStyle = BorderLineStyleType.None.ToString(),
                    BorderLeftStyle = BorderLineStyleType.None.ToString(),
                    BorderRightStyle = BorderLineStyleType.Normal.ToString()
                }
            };
        }
        #endregion

        #region Создать модель колонок
        /// <summary>
        /// Создать модель колонок
        /// </summary>
        /// <param name="startCol"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ModelColumn> CreateModelColumns(int startCol, int count)
        {
            var array = new List<ModelColumn>();
            for (int i = startCol; i < count; i++)
                array.Add(CreateModelColumn(i, GetHeaderColumn(i + 1)));
            return array;
        }
        #endregion

        #region Удалить модель колонки
        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        public void DeleteModelColumn()
            => DeleteModelColumn(SelectedModel.SelectedModelColumn);

        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        /// <param name="column"></param>
        public void DeleteModelColumn(ModelColumn column)
        {
            SelectedModel.Columns.Remove(column);
            if (SelectedModel.SelectedModelColumn.Equals(column))
                SelectedModel.SelectedModelColumn = Helper.GetSelectedElement<ModelColumn>(column.Index, SelectedModel.Columns);
        }

        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        /// <param name="columns"></param>
        public void DeleteModelColumn(ObservableCollection<ModelColumn> columns)
        {
            foreach (var col in columns)
                DeleteModelColumn(col);
        }
        #endregion

        #region Создать модель строки
        /// <summary>
        /// Создать модель строки
        /// </summary>
        /// <param name="index"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public ModelRow CreateModelRow(int index, string header)
        {
            return new()
            {
                Id = header,
                Index = index,
                Header = header,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                Cells = [.. CreateModelCells(index, 0, ColumnCountDefault)],
                Geometry = new Models.Geometry()
                {
                    Width = RowWidthDefault,
                    Height = RowHeightDefault,
                    PositionX = 0,
                    PositionY = index * RowHeightDefault,
                },
                CellStyle = new()
                {
                    Background = Helper.GetColor(Brushes.Transparent),
                    Foreground = Helper.GetColor(Brushes.LightGray),
                    BorderBrush = Helper.GetColor(Brushes.LightGray),
                    FontFamily = FontFamilyHelper.AppFontDefault.Name,
                    FontSize = FontFamilyHelper.FontSizeDefault,
                    IsBold = true,
                    IsItalic = false,
                    IsUnderline = false,
                    IsWrap = false,
                    HorizontalContentAlignment = HorizontalAlignment.Center.ToString(),
                    VerticalContentAlignment = VerticalAlignment.Center.ToString(),
                    BorderBottomStyle = BorderLineStyleType.Normal.ToString(),
                    BorderTopStyle = BorderLineStyleType.None.ToString(),
                    BorderLeftStyle = BorderLineStyleType.None.ToString(),
                    BorderRightStyle = BorderLineStyleType.Normal.ToString()
                }
            };
        }
        #endregion

        #region Создать модель строк
        /// <summary>
        /// Создать модель строк
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="count"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public List<ModelRow> CreateModelRows(int startRow, int count)
        {
            var array = new List<ModelRow>();
            for (int i = startRow; i < count; i++)
                array.Add(CreateModelRow(i, GetHeaderRow(i + 1)));

            return array;
        }
        #endregion

        #region Удалить модель строки
        /// <summary>
        /// Удалить модель строки
        /// </summary>
        public void DeleteModelRowRow()
            => DeleteModelRowRow(SelectedModel.SelectedModelRow);

        /// <summary>
        /// Удалить модель строки
        /// </summary>
        /// <param name="row"></param>
        public void DeleteModelRowRow(ModelRow row)
        {
            SelectedModel.Rows.Remove(row);
            if (SelectedModel.SelectedModelRow.Equals(row))
                SelectedModel.SelectedModelRow = Helper.GetSelectedElement<ModelRow>(row.Index, SelectedModel.Rows);
        }

        /// <summary>
        /// Удалить модель строки
        /// </summary>
        /// <param name="rows"></param>
        public void DeleteModelRowRow(ObservableCollection<ModelRow> rows)
        {
            foreach (var row in rows)
                DeleteModelRowRow(row);
        }
        #endregion

        #region Создать модель ячейкм
        /// <summary>
        /// Создать модель ячейкм
        /// </summary>
        /// <param name="indexCol"></param>
        /// <param name="indexRow"></param>
        /// <returns></returns>
        public ModelCell CreateModelCell(int indexCol, int indexRow)
        {
            return new()
            {
                Id = $"{GetHeaderColumn(indexCol + 1)}{indexRow}",
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                ColumnIndex = indexCol,
                RowIndex = indexRow,
                RawValue = "",
                Geometry = new Models.Geometry()
                {
                    Width = ColumnWidthDefault,
                    Height = RowHeightDefault,
                    PositionX = indexCol * ColumnWidthDefault,
                    PositionY = indexRow * RowHeightDefault,
                },
                CellStyle = new()
                {
                    Background = Helper.GetColor(Brushes.Transparent),
                    Foreground = Helper.GetColor(Brushes.Black),
                    BorderBrush = Helper.GetColor(Brushes.LightGray),
                    FontFamily = FontFamilyHelper.AppFontDefault.Name,
                    FontSize = FontFamilyHelper.FontSizeDefault,
                    IsBold = false,
                    IsItalic = false,
                    IsUnderline = false,
                    IsWrap = false,
                    HorizontalContentAlignment = HorizontalAlignment.Center.ToString(),
                    VerticalContentAlignment = VerticalAlignment.Center.ToString(),
                    BorderBottomStyle = BorderLineStyleType.None.ToString(),
                    BorderTopStyle = BorderLineStyleType.None.ToString(),
                    BorderLeftStyle = BorderLineStyleType.None.ToString(),
                    BorderRightStyle = BorderLineStyleType.None.ToString()
                }
            };
        }
        #endregion

        #region Создать модель ячеек
        /// <summary>
        /// Создать модель ячеек
        /// </summary>
        /// <param name="indexRow"></param>
        /// <param name="startCell"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ModelCell> CreateModelCells(int indexRow, int startCell, int count)
        {
            var array = new List<ModelCell>();
            for (int i = startCell; i < count; i++)
                array.Add(CreateModelCell(i, indexRow));

            return array;
        }
        #endregion

        #region Удалить модель ячейки
        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        public void DeleteCells()
            => DeleteCells(SelectedModel.SelectedModelRow, SelectedModel.SelectedModelCell);

        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        public void DeleteCells(ModelRow row, ModelCell cell)
        {
            var index = row.Cells.IndexOf(cell);
            row.Cells.Remove(cell);
            if (SelectedModel.SelectedModelCell.Equals(cell))
                SelectedModel.SelectedModelCell = Helper.GetSelectedElement<ModelCell>(index, row.Cells);
        }

        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cells"></param>
        public void DeleteCells(ModelRow row, ObservableCollection<ModelCell> cells)
        {
            foreach (var cell in cells)
                DeleteCells(row, cell);
        }
        #endregion

        #region Обновить визульное пространство
        /// <summary>
        /// Обновить визульное пространство
        /// </summary>
        public void UpdateViewport()
        {
            UpdateElementsVisible(
                SelectedModel.PositionX,
                SelectedModel.PositionY,
                ConnectorService.WindowWidth,
                ConnectorService.WindowHeight
                );

            ScrollBarService.UpdateViewport(
                 ConnectorService.WindowWidth,
                 ConnectorService.WindowHeight,
                 SelectedModel.Width,
                 SelectedModel.Height
                 );


            SelectedModel?.UpdateGeometryColumnsFinished?.Invoke();
            SelectedModel?.UpdateGeometryRowsFinished?.Invoke();
            SelectedModel?.UpdateGeometryCellsFinished?.Invoke();
        }
        #endregion

        #region Обновить область отображения изменения размера
        /// <summary>
        /// Обновить область отображения изменения размера
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="positionStart"></param>
        /// <param name="positionEnd"></param>
        public void UpdateLayoutDrag(Orientation orientation, double positionStart, double positionEnd)
        {
            SelectedModel.DragAreaChangeEvent.Invoke(
                orientation,
                GetRect(orientation, positionStart),
                GetRect(orientation, positionEnd)
                );

            switch (orientation)
            {
                case Orientation.Vertical:
                    SelectedModel.UpdateGeometryColumnsFinished.Invoke();
                    break;
                case Orientation.Horizontal:
                    SelectedModel.UpdateGeometryRowsFinished.Invoke();
                    break;
            }
        }
        #endregion

        #region Обработка события начала редактирования ячейки
        /// <summary>
        /// Обработка события начала редактирования ячейки
        /// </summary>
        private void OnEditChangeEvent(AppActiveModeType activeMode)
            => ConnectorService.AppActiveMode = activeMode;
        #endregion

        #region Изменение размера завершено
        /// <summary>
        /// Изменение размера завершено
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="item"></param>
        public void UpdateLayoutDragComplete(Orientation orientation, ModelColumn item)
        {
            UpdateHorizontalPosition(item.Index);
            SelectedModel.UpdateGeometryColumnsFinished.Invoke();
            SelectedModel.UpdateGeometryCells();

            SelectedModel.Width = SelectedModel.Columns.Sum(x => x.Geometry.Width);
            UpdateViewport();

            SelectedModel.DragAreaChangeEvent.Invoke(orientation, null, null);
        }

        /// <summary>
        /// Изменение размера завершено
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="item"></param>
        public void UpdateLayoutDragComplete(Orientation orientation, ModelRow item)
        {
            UpdateVerticalPosition(item.Index);
            SelectedModel.UpdateGeometryRowsFinished.Invoke();
            SelectedModel.UpdateGeometryCells();

            SelectedModel.Height = SelectedModel.Rows.Sum(x => x.Geometry.Height);
            UpdateViewport();

            SelectedModel.DragAreaChangeEvent.Invoke(orientation, null, null);
        }
        #endregion

        #region Установить размер колонок по содержимому
        /// <summary>
        /// Установить размер колонок по содержимому
        /// </summary>
        /// <param name="item"></param>
        public void SetSizeColumnToContent(ModelColumn item)
        {
            var width = Helper.MeasureTextWidth(item.Header, item.CellStyle.FontSize, item.CellStyle.FontFamily);
            item.Geometry.Width = width;

            UpdateHorizontalPosition(item.Index);
            UpdateLayoutDrag(Orientation.Vertical, item.Geometry.PositionX, item.Geometry.Right);
        }
        #endregion

        #region Установить размер строк по содержимому
        /// <summary>
        /// Установить размер строк по содержимому
        /// </summary>
        /// <param name="item"></param>
        public void SetSizeRowToContent(ModelRow item)
        {
            var height = Helper.MeasureTextHeight(item.Header, item.CellStyle.FontSize, item.CellStyle.FontFamily);
            item.Geometry.Height = height;

            UpdateVerticalPosition(item.Index);
            UpdateLayoutDrag(Orientation.Horizontal, item.Geometry.PositionY, item.Geometry.Bottom);
        }
        #endregion

        #region Навигация по таблице
        /// <summary>
        /// Навигация по таблице
        /// </summary>
        /// <param name="e"></param>
        public void NavigationTable(KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key != Key.Delete)
            {
                if (e.Key == Key.Right || e.Key == Key.Tab)
                    SelectedModel.SelectNextCell(NavigationNextType.Right);
                else if (e.Key == Key.Left)
                    SelectedModel.SelectNextCell(NavigationNextType.Left);
                else if (e.Key == Key.Up)
                    SelectedModel.SelectNextCell(NavigationNextType.Top);
                else if (e.Key == Key.Down || e.Key == Key.Return)
                {
                    if (e.KeyModifiers == KeyModifiers.None)
                        SelectedModel.SelectNextCell(NavigationNextType.Bottom);
                }

                var posX = ScrollBarService.HorizontalScrollBarValue;
                var right = ScrollBarService.HorizontalScrollViewportSize + posX;
                if (SelectedModel.SelectedModelCell.Geometry.Right >= right - SelectedModel.SelectedModelCell.Geometry.Width)
                {
                    var offset = posX + SelectedModel.SelectedModelCell.Geometry.Width * 3;
                    ScrollBarService.SetHorizontalScrollBarValue(offset);
                }
                else if (SelectedModel.SelectedModelCell.Geometry.PositionX <= posX)
                {
                    var offset = posX - SelectedModel.SelectedModelCell.Geometry.Width * 3;
                    ScrollBarService.SetHorizontalScrollBarValue(offset);
                }

                var posY = ScrollBarService.VerticalScrollBarValue;
                var bottom = ScrollBarService.VerticalScrollViewportSize + posY - 50;
                if (SelectedModel.SelectedModelCell.Geometry.Bottom >= bottom - SelectedModel.SelectedModelCell.Geometry.Height)
                {
                    var offset = posY + SelectedModel.SelectedModelCell.Geometry.Height * 5;
                    ScrollBarService.SetVerticalScrollBarValue(offset);
                }
                else if (SelectedModel.SelectedModelCell.Geometry.PositionY <= posY)
                {
                    var offset = posY - SelectedModel.SelectedModelCell.Geometry.Height * 3;
                    ScrollBarService.SetVerticalScrollBarValue(offset);
                }
            }
            else
            {
                SelectedModel.SelectedModelCells?.Where(x => !string.IsNullOrWhiteSpace(x.RawValue))?
                    .ToList()?
                    .ForEach(x => x.RawValue = "");
            }
        }
        #endregion

        #region Ввод данных
        /// <summary>
        /// Ввод данных
        /// </summary>
        /// <param name="e"></param>
        public void InputValue(KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.F2)
                SelectedModel.SelectedModelCell.IsEdit = true;
            else
            {
                var value = "";
                if (e.Key != Key.Space && e.Key != Key.Back)
                    value = e.KeySymbol;

                SelectedModel.SelectedModelCell.RawValue = value;
                SelectedModel.SelectedModelCell.IsEdit = true;
            }
        }
        #endregion





















        #region Получить заголовок колонки
        /// <summary>
        /// Получить заголовок колонки
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetHeaderColumn(int index)
        {
            var name = string.Empty;
            while (index > 0)
            {
                index--;
                name = (char)('A' + (index % 26)) + name;
                index /= 26;
            }
            return name;
        }
        #endregion

        #region Получить заголовок строки
        /// <summary>
        /// Получить заголовок строки
        /// </summary>
        /// <returns></returns>
        private static string GetHeaderRow(int index)
            => $"{index}";
        #endregion

        #region Создать заголовок
        /// <summary>
        /// Создать заголовок
        /// </summary>
        /// <returns></returns>
        private string CreateHeader()
        {
            var header = $"Таблица {Models.Count + 1}";
            if (Models.FirstOrDefault(x => x.Id.Equals(header, StringComparison.InvariantCultureIgnoreCase)) is { })
            {
                var index = 1;
                header = $"Таблица {index}";
                while (IsEquals(header))
                {
                    index++;
                    header = $"Таблица {index}";
                }
            }
            return header;

            bool IsEquals(string id)
                => Models?
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)) is { };
        }
        #endregion









        #region Получить Rect
        /// <summary>
        /// Получить Rect
        /// </summary>
        /// <param name="Flow"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private Rect GetRect(Orientation Flow, double position)
        {
            var rect = new Rect();
            switch (Flow)
            {
                case Orientation.Vertical:
                    var width = 1d;
                    double height = ConnectorService.WindowHeight;
                    rect = new(position - ScrollBarService.HorizontalScrollBarValue, 0, width, height);
                    break;

                case Orientation.Horizontal:
                    width = ConnectorService.WindowWidth;
                    height = 1d;
                    rect = new(0, position - ScrollBarService.VerticalScrollBarValue, width, height);
                    break;
            }
            return rect;
        }
        #endregion

        #region Обновить горизонтальные позиции после изменения размеров колонок
        /// <summary>
        /// Обновить горизонтальные позиции после изменения размеров колонок
        /// </summary>
        /// <param name="index"></param>
        private void UpdateHorizontalPosition(int index)
        {
            if (SelectedModel.Columns[index].IsHeader)
            {
                foreach (var column in SelectedModel.SelectedModelColumns)
                {
                    column.Geometry.Width = SelectedModel.Columns[index].Geometry.Width;
                }
                index = SelectedModel.SelectedModelColumns[0].Index;
            }
            var posX = SelectedModel.Columns[index].Geometry.PositionX;
            for (int i = index; i < SelectedModel.Columns.Count; i++)
            {
                SelectedModel.Columns[i].Geometry.PositionX = posX;
                for (int j = 0; j < SelectedModel.Rows.Count; j++)
                {
                    SelectedModel.Rows[j].Cells[i].Geometry.PositionX = posX;
                    posX = SelectedModel.Rows[j].Cells[i].Geometry.Right;
                }
                posX = SelectedModel.Columns[i].Geometry.Right;
            }
        }
        #endregion

        #region Обновить вертикальные позиции после изменения размеров строк
        /// <summary>
        /// Обновить вертикальные позиции после изменения размеров строк
        /// </summary>
        /// <param name="index"></param>
        private void UpdateVerticalPosition(int index)
        {
            if (SelectedModel.Rows[index].IsHeader)
            {
                foreach (var row in SelectedModel.SelectedModelRows)
                {
                    row.Geometry.Height = SelectedModel.Rows[index].Geometry.Height;
                }
                index = SelectedModel.SelectedModelRows[0].Index;
            }

            var posY = SelectedModel.Rows[index].Geometry.PositionY;
            for (int i = index; i < SelectedModel.Rows.Count; i++)
            {
                SelectedModel.Rows[i].Geometry.PositionY = posY;
                for (int j = 0; j < SelectedModel.Rows[i].Cells.Count; j++)
                    SelectedModel.Rows[i].Cells[j].Geometry.PositionY = posY;

                posY = SelectedModel.Rows[i].Geometry.Bottom;
            }
        }
        #endregion

        #region Обновить видимые элементы
        /// <summary>
        /// Обновить видимые элементы
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void UpdateElementsVisible(double x, double y, double width, double height)
        {
            double viewWidth = x + width + ColumnWidthDefault * 10;
            double viewHeight = y + height + RowHeightDefault * 20;
            var posX = x - ColumnWidthDefault * 5;
            var posY = y - RowHeightDefault * 10;

            SelectedModel.ColumnsVisible = [.. SelectedModel.Columns.Where(col
                => col.Geometry.PositionX >= posX && col.Geometry.Right <= viewWidth)?.ToList()];

            SelectedModel.RowsVisible = [.. SelectedModel.Rows.Where(row
                => row.Geometry.PositionY >= posY && row.Geometry.Bottom <= viewHeight)?.ToList()];

            SelectedModel.CellsVisible = [.. SelectedModel.Cells.Where(cell
                => cell.Geometry.PositionX >= posX && cell.Geometry.Right <= viewWidth
                && cell.Geometry.PositionY >= posY && cell.Geometry.Bottom <= viewHeight)?.ToList()];
        }
        #endregion

        #region Обработка изменения масштаба
        /// <summary>
        /// Обработка изменения масштаба
        /// </summary>
        /// <param name="scale"></param>
        private void OnScaleChange(double scale)
        {
            SelectedModel.Scale = Convert.ToInt32(scale / 100);
            //SelectedModel.HeaderColumnsHeight = ColumnHeightDefault * scale;
            //SelectedModel.HeaderRowsWidth = RowWidthDefault * scale;

            //var posX = 0d;
            //for (int i = 0; i < SelectedModel.Columns.Count; i++)
            //{
            //    SelectedModel.Columns[i].PositionX = posX;
            //    SelectedModel.Columns[i].Width = SelectedModel.Columns[i].WidthResult * scale;
            //    SelectedModel.Columns[i].Right = SelectedModel.Columns[i].PositionX + SelectedModel.Columns[i].Width;
            //    posX = SelectedModel.Columns[i].Right;
            //}

            //UpdateHorizontalPosition(0);



            //FrameColumns.ColsX.Clear();
            //foreach (var column in model.Columns.Where(x => x.IsVisible))
            //{
            //    FrameColumns.ColsX.Add(new()
            //    {
            //        PositionX = column.PositionX,
            //        PositionY = column.PositionY,
            //        Right = column.Right,
            //        Bottom = column.Bottom,
            //        Size = column.Height,
            //        LinePen = new(Brushes.WhiteSmoke)
            //    });
            //}
            //FrameColumns.InvalidateVisual();

        }
        #endregion

        #region Обработка изменения выбранного элемента
        /// <summary>
        /// Обработка изменения выбранного элемента
        /// </summary>
        /// <param name="item"></param>
        private void OnSelectedItemChange(ModelTable item)
        {
            SelectedModel = item;
            if (SelectedModel.SelectedModelCell is not { })
            {
                Dispatcher.UIThread.Post(() =>
                {
                    SelectedModel.SetSelected(SelectedModel.Cells[0]);

                }, DispatcherPriority.ContextIdle);
            }
            UpdateViewport();
        }
        #endregion




    }
}