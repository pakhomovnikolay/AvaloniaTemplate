using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;
using AvaloniaTemplate.Services;
using AvaloniaTemplate.Services.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class PresenterColumnsHelper
    {
        private static readonly StartIndex CurrentIndex = new();
        private static readonly SelectionService<ModelColumn> SelectorColumns = new(() => TableFactory.SelectedModel.Columns, x => x.Index);
        private static readonly  ITableGenerateFactory TableFactory = App.GetService<ITableGenerateFactory>();

        static PresenterColumnsHelper()
        {
            SelectedChangeControlProperty.Changed.AddClassHandler<PresenterColumns>(RegisterSelectedChangeControl);
            SelectorColumns.SelectedChangedItem += OnSelectedChangedColumn;
            SelectorColumns.SelectedChangedItems += OnSelectedChangedColumns;
            SelectorColumns.MultiSelectedChangedItem += OmMultiSelectedChangedColumn;
            SelectorColumns.SelectedAreaChanged += OnColumnSelectedAreaChanged;
        }

        public static readonly AttachedProperty<bool> SelectedChangeControlProperty
            = AvaloniaProperty.RegisterAttached<PresenterColumnsHelper, Control, bool>("SelectedChangeControl");

        public static void SetSelectedChangeControl(Control control, bool value)
            => control.SetValue(SelectedChangeControlProperty, value);

        public static bool GetSelectedChangeControl(Control control)
            => control.GetValue(SelectedChangeControlProperty);

        private static void RegisterSelectedChangeControl(PresenterColumns presenter, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
            {
                presenter.SelectedItemChanged += OnSelectedItemChanged;
                presenter.SetFocusItem += SelectorColumns.SetFocus;
                presenter.ResetFocusItem += SelectorColumns.ResetFocus;
                presenter.PointerMovedEventChange += OnPointerMovedEventChange;
            }
            else
            {
                presenter.SelectedItemChanged -= OnSelectedItemChanged;
                presenter.SetFocusItem -= SelectorColumns.SetFocus;
                presenter.ResetFocusItem -= SelectorColumns.ResetFocus;
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
        private static void OnSelectedItemChanged(Control s, PointerPressedEventArgs e, ModelColumn item)
        {
            CurrentIndex.IsEqaulColumn(item.Index);
            CurrentIndex.Column = item.Index;
            SelectorColumns.SetSelected(e, item);
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
            if (CurrentIndex.IsEqaulColumn(GetColumn(point.X))
                || CurrentIndex.CurrentColumn < 0
                || CurrentIndex.CurrentColumn >= TableFactory.SelectedModel.Columns.Count
                )
                return;

            SelectorColumns.SetRangeSelected(CurrentIndex.CurrentColumn);
        }
        #endregion

        #region Событие изменения текущей колонки
        /// <summary>
        /// Событие изменения текущей колонки
        /// </summary>
        /// <param name="item"></param>
        private static void OnSelectedChangedColumn(ModelColumn item)
        {
            TableFactory.SelectedModel.SelectedModelColumns.Clear();
            TableFactory.SelectedModel.SelectedModelColumns.Add(item);
            TableFactory.SelectedModel.SelectedModelColumn = item;
        }
        #endregion

        #region Событие изменения выбранных колонок
        /// <summary>
        /// Событие изменения выбранных колонок
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private static void OnSelectedChangedColumns(IEnumerable<ModelColumn> added, IEnumerable<ModelColumn> removed)
        {
            if (removed is { } && removed.Any())
                foreach (var item in removed)
                    TableFactory.SelectedModel.SelectedModelColumns.Remove(item);

            if (added is { } && added.Any())
                foreach (var item in added)
                    TableFactory.SelectedModel.SelectedModelColumns.Add(item);

            TableFactory.SelectedModel.UpdateSelectedModelColumns();
        }
        #endregion

        #region Событие изменения выбранной колонки
        /// <summary>
        /// Событие изменения выбранной колонки
        /// </summary>
        /// <param name="item"></param>
        private static void OmMultiSelectedChangedColumn(ModelColumn item, bool remove)
        {
            if (remove)
                TableFactory.SelectedModel.SelectedModelColumn = TableFactory.SelectedModel.SelectedModelColumns?
                    .FirstOrDefault(x => !x.Equals(item));
            else
                TableFactory.SelectedModel.SelectedModelColumn = item;


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

        #region Получить индекс колонки
        /// <summary>
        /// Получить индекс колонки
        /// </summary>
        /// <returns></returns>
        private static int GetColumn(double posX)
        {
            int left = 0;
            int right = TableFactory.SelectedModel.Columns.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                var col = TableFactory.SelectedModel.Columns[mid];

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
