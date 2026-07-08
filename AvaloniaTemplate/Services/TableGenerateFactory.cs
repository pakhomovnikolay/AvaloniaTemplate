using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaTemplate.Services
{
    public class TableGenerateFactory : ObservableObject, ITableGenerateFactory
    {
        private const double ColumnWidthDefault = 70;
        private const double ColumnHeightDefault = 30;
        private const double RowWidthDefault = 30;
        private const double RowHeightDefault = 25;
        private const int ColumnCountDefault = 50;
        private const int RowCountDefault = 45;

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

        public TableGenerateFactory()
        {
            ZoomService.ScaleChange += (v) => SelectedModel.Scale = Convert.ToInt32(v / 100);
            TabStripService.CreateItem += CreateModel;
            TabStripService.SelectedItemChange += OnSelectedItemChange;
            if (Models is not { })
            {
                Models ??= [];
                TabStripService.ItemsSource = Models;
                TabStripService.Command_CreateItem.Execute(Models);
            }
        }

        private void OnSelectedItemChange(ModelTable item)
        {
            SelectedModel = item;
            ScrollBarService.UpdateViewport(ConnectorService.WindowWidth, ConnectorService.WindowHeight, SelectedModel.Width, SelectedModel.Height);
        }

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
        private ObservableCollection<ModelColumn> selectedModelColumns;
        /// <summary>
        /// Выбранные колонки
        /// </summary>
        public ObservableCollection<ModelColumn> SelectedModelColumns
        {
            get => selectedModelColumns;
            set => SetProperty(ref selectedModelColumns, value);
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
        private ObservableCollection<ModelRow> selectedModelRows;
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
        private ObservableCollection<ModelCell> selectedModelCells;
        /// <summary>
        /// Выбранные ячейки
        /// </summary>
        public ObservableCollection<ModelCell> SelectedModelCells
        {
            get => selectedModelCells;
            set => SetProperty(ref selectedModelCells, value);
        }
        #endregion

        #region Создать модель
        /// <summary>
        /// Создать модель
        /// </summary>
        public ModelTable CreateModel()
        {
            var header = CreateHeader();
            return new()
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
                Width = ColumnWidthDefault,
                Height = ColumnHeightDefault,
                PositionX = index * ColumnWidthDefault,
                PositionY = 0,
                Right = index * ColumnWidthDefault + ColumnWidthDefault,
                Bottom = ColumnHeightDefault,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                CellStyle = new()
                {
                    Background = Helper.GetColor(Brushes.Transparent),
                    Foreground = Helper.GetColor(Brushes.Black),
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
            => DeleteModelColumn(SelectedModelColumn);

        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        /// <param name="column"></param>
        public void DeleteModelColumn(ModelColumn column)
        {
            SelectedModel.Columns.Remove(column);
            if (SelectedModelColumn.Equals(column))
                SelectedModelColumn = Helper.GetSelectedElement<ModelColumn>(column.Index, SelectedModel.Columns);
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
                Width = RowWidthDefault,
                Height = RowHeightDefault,
                PositionX = 0,
                PositionY = index * RowHeightDefault,
                Right = RowWidthDefault,
                Bottom = index * RowHeightDefault + RowHeightDefault,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                Cells = [.. CreateModelCells(index, 0, ColumnCountDefault)],
                CellStyle = new()
                {
                    Background = Helper.GetColor(Brushes.Transparent),
                    Foreground = Helper.GetColor(Brushes.Black),
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
                array.Add(CreateModelRow(i, GetHeaderRow(i)));

            return array;
        }
        #endregion

        #region Удалить модель строки
        /// <summary>
        /// Удалить модель строки
        /// </summary>
        public void DeleteModelRowRow()
            => DeleteModelRowRow(SelectedModelRow);

        /// <summary>
        /// Удалить модель строки
        /// </summary>
        /// <param name="row"></param>
        public void DeleteModelRowRow(ModelRow row)
        {
            SelectedModel.Rows.Remove(row);
            if (SelectedModelRow.Equals(row))
                SelectedModelRow = Helper.GetSelectedElement<ModelRow>(row.Index, SelectedModel.Rows);
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
                Width = ColumnWidthDefault,
                Height = ColumnHeightDefault,
                PositionX = indexCol * ColumnWidthDefault,
                PositionY = indexRow * ColumnHeightDefault,
                Right = indexCol * ColumnWidthDefault + ColumnWidthDefault,
                Bottom = indexRow * ColumnHeightDefault + ColumnHeightDefault,
                IsVisible = true,
                IsSelected = false,
                IsFocused = false,
                ColumnIndex = indexCol,
                RowIndex = indexRow,
                RawValue = "",
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
            => DeleteCells(SelectedModelRow, SelectedModelCell);

        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        public void DeleteCells(ModelRow row, ModelCell cell)
        {
            var index = row.Cells.IndexOf(cell);
            row.Cells.Remove(cell);
            if (SelectedModelCell.Equals(cell))
                SelectedModelCell = Helper.GetSelectedElement<ModelCell>(index, row.Cells);
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
    }
}