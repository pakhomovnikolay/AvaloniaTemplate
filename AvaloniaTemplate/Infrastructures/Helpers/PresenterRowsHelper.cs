using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;
using AvaloniaTemplate.Services;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class PresenterRowsHelper
    {
        private static readonly StartIndex CurrentIndex = new();
        private static readonly SelectionService<ModelRow> SelectorRows = new(() => TableFactory.SelectedModel.Rows, x => x.Index);
        private static readonly ITableGenerateFactory TableFactory = App.GetService<ITableGenerateFactory>();

        static PresenterRowsHelper()
        {
            SelectedChangeControlProperty.Changed.AddClassHandler<PresenterRows>(RegisterSelectedChangeControl);
            SelectorRows.SelectedChangedItem += OnSelectedChangedColumn;
            SelectorRows.SelectedChangedItems += OnSelectedChangedColumns;
            SelectorRows.MultiSelectedChangedItem += OnMultiSelectedChangedRow;
            SelectorRows.SelectedAreaChanged += OnColumnSelectedAreaChanged;
        }

        public static readonly AttachedProperty<bool> SelectedChangeControlProperty
            = AvaloniaProperty.RegisterAttached<PresenterRowsHelper, Control, bool>("SelectedChangeControl");

        public static void SetSelectedChangeControl(Control control, bool value)
            => control.SetValue(SelectedChangeControlProperty, value);

        public static bool GetSelectedChangeControl(Control control)
            => control.GetValue(SelectedChangeControlProperty);

        private static void RegisterSelectedChangeControl(PresenterRows presenter, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
            {
                presenter.SelectedItemChanged += OnSelectedItemChanged;
                presenter.SetFocusItem += SelectorRows.SetFocus;
                presenter.ResetFocusItem += SelectorRows.ResetFocus;
                presenter.PointerMovedEventChange += OnPointerMovedEventChange;
            }
            else
            {
                presenter.SelectedItemChanged -= OnSelectedItemChanged;
                presenter.SetFocusItem -= SelectorRows.SetFocus;
                presenter.ResetFocusItem -= SelectorRows.ResetFocus;
                presenter.PointerMovedEventChange -= OnPointerMovedEventChange;
            }
        }

        #region Событие смены выбора элемента
        /// <summary>
        /// Событие смены выбора элемента
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <param name="item"></param>
        private static void OnSelectedItemChanged(PointerPressedEventArgs e, ModelRow item)
        {
            CurrentIndex.IsEqaulRow(item.Index);
            CurrentIndex.Row = item.Index;
            SelectorRows.SetSelected(e, item);
        }
        #endregion

        #region Событие перемешения указателя по главной панели
        /// <summary>
        /// Событие перемешения указателя по главной панели
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private static void OnPointerMovedEventChange(Control? s, PointerEventArgs e)
        {
            var point = e.GetPosition(s);
            if (CurrentIndex.IsEqaulRow(GetRow(point.Y))
                || CurrentIndex.CurrentRow < 0
                || CurrentIndex.CurrentRow >= TableFactory.SelectedModel.Rows.Count
                )
                return;

            SelectorRows.SetRangeSelected(CurrentIndex.CurrentRow);
        }
        #endregion

        #region Событие изменения текущей строки
        /// <summary>
        /// Событие изменения текущей строки
        /// </summary>
        /// <param name="item"></param>
        private static void OnSelectedChangedColumn(ModelRow item)
        {
            TableFactory.SelectedModel.SelectedModelRows.Clear();
            TableFactory.SelectedModel.SelectedModelRows.Add(item);
            TableFactory.SelectedModel.SelectedModelRow = item;

            TableFactory.SelectedModel.SelectedModelColumns.Clear();
            TableFactory.SelectedModel.SelectedModelColumn = null;
        }
        #endregion

        #region Событие изменения выбранных строки
        /// <summary>
        /// Событие изменения выбранных строки
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private static void OnSelectedChangedColumns(IEnumerable<ModelRow> added, IEnumerable<ModelRow> removed)
        {
            if (removed is { } && removed.Any())
                foreach (var item in removed)
                    TableFactory.SelectedModel.SelectedModelRows.Remove(item);

            if (added is { } && added.Any())
                foreach (var item in added)
                    TableFactory.SelectedModel.SelectedModelRows.Add(item);

            TableFactory.SelectedModel.UpdateSelectedModelRows();
        }
        #endregion

        #region Событие изменения выбранной строки
        /// <summary>
        /// Событие изменения выбранной строки
        /// </summary>
        /// <param name="item"></param>
        private static void OnMultiSelectedChangedRow(ModelRow item, bool remove)
        {
            if (remove)
                TableFactory.SelectedModel.SelectedModelRow = TableFactory.SelectedModel.SelectedModelRows?
                    .FirstOrDefault(x => !x.Equals(item));
            else
                TableFactory.SelectedModel.SelectedModelRow = item;


            //var array = Cells.Where(x => x.IndexColumn == item.Index && x.IndexRow < Rows.Count);
            //if (remove)
            //{
            //    SelectedColumn = SelectedColumns.FirstOrDefault(x => !x.Equals(item));
            //    if (SelectedColumn is { })
            //        SelectedCell = Cells.FirstOrDefault(x => x.IndexColumn == SelectedColumn.Index && x.IndexRow == 0);
            //    else if (SelectedCells?.Count > 0)
            //        SelectedCell = SelectedCells?.FirstOrDefault();

            //    UpdateFocusCells([SelectedCell]);
            //    UpdateSelectedCells([], array);
            //}
            //else
            //{
            //    SelectedColumn = item;
            //    SelectedCell = Cells.FirstOrDefault(x => x.IndexColumn == SelectedColumn.Index && x.IndexRow == 0);

            //    UpdateFocusCells([SelectedCell]);
            //    UpdateSelectedCells(array, []);
            //    if (Rows.FirstOrDefault(x => x.IsSelected) is not { })
            //        UpdateSelectedRows(Rows);
            //}
        }
        #endregion

        #region Событие изменения выделенной области колонок
        /// <summary>
        /// Событие изменения выделенной области колонок
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="delete"></param>
        private static void OnColumnSelectedAreaChanged(Rect? arg1, bool delete)
        {
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

        #region Получить индекс строки
        /// <summary>
        /// Получить индекс строки
        /// </summary>
        /// <returns></returns>
        private static int GetRow(double posY)
        {
            int top = 0;
            int bottom = TableFactory.SelectedModel.Rows.Count - 1;

            while (top <= bottom)
            {
                int mid = (top + bottom) / 2;
                var row = TableFactory.SelectedModel.Rows[mid];

                if (posY < row.PositionY)
                    bottom = mid - 1;
                else if (posY > row.PositionY + row.Height)
                    top = mid + 1;
                else
                    return mid;
            }
            return -1;
        }
        #endregion
    }
}
