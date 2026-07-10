using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;
using AvaloniaTemplate.Services;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class PresenterColumnsHelper
    {
        private StartIndex CurrentIndex { get; } = new();
        private ITableGenerateFactory TableFactory { get; } = App.GetService<ITableGenerateFactory>();
        private SelectionService<ModelColumn> SelectorColumns { get; }
        public PresenterColumnsHelper() { }
        public PresenterColumnsHelper(PresenterColumns presenter)
        {
            SelectorColumns = new SelectionService<ModelColumn>(() => TableFactory.SelectedModel.Columns, x => x.Index);
            SelectorColumns.SelectedChangedItem += OnSelectedItemChanged;
            SelectorColumns.SelectedChangedItems += OnSelectedItemsChanged;
            SelectorColumns.MultiSelectedChangedItem += OnMultiSelectedItemChanged;
            SelectorColumns.SelectedAreaChanged += OnSelectedAreaChanged;

            presenter.SelectedItemChanged += SelectedItemChanged;
            presenter.SetFocusItem += SelectorColumns.SetFocus;
            presenter.ResetFocusItem += SelectorColumns.ResetFocus;
            presenter.PointerMovedEventChange += PointerMovedEventChange;
            presenter.DragStartedEvent += OnDragStartedEvent;
            presenter.WidthChangeEvent += OnWidthChangeEvent;
            presenter.DragCompletedEvent += OnDragCompletedEvent;
            presenter.SizeToContentEvent += OnSizeToContentEvent;
        }
        static PresenterColumnsHelper()
        {
            SelectedChangeControlProperty.Changed.AddClassHandler<PresenterColumns>(RegisterSelectedChangeControl);
        }

        public static readonly AttachedProperty<bool> SelectedChangeControlProperty
            = AvaloniaProperty.RegisterAttached<PresenterColumnsHelper, Control, bool>("SelectedChangeControl");

        public static void SetSelectedChangeControl(Control control, bool value)
            => control.SetValue(SelectedChangeControlProperty, value);

        public static bool GetSelectedChangeControl(Control control)
            => control.GetValue(SelectedChangeControlProperty);

        private static void RegisterSelectedChangeControl(PresenterColumns presenter, AvaloniaPropertyChangedEventArgs e)
        {
            if (Design.IsDesignMode)
                return;

            if (e.NewValue is bool)
                _ = new PresenterColumnsHelper(presenter);
        }

        #region Событие смены выбора элемента
        /// <summary>
        /// Событие смены выбора элемента
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <param name="item"></param>
        private void SelectedItemChanged(PointerPressedEventArgs e, ModelColumn item)
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
        private void PointerMovedEventChange(Control? s, PointerEventArgs e)
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
        private void OnSelectedItemChanged(ModelColumn item)
        {
            TableFactory.SelectedModel.SelectedModelColumns.Clear();
            TableFactory.SelectedModel.SelectedModelColumns.Add(item);
            TableFactory.SelectedModel.SelectedModelColumn = item;

            TableFactory.SelectedModel.SelectedModelRows.Clear();
            TableFactory.SelectedModel.SelectedModelRow = null;
        }
        #endregion

        #region Событие изменения выбранных колонок
        /// <summary>
        /// Событие изменения выбранных колонок
        /// </summary>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        private void OnSelectedItemsChanged(IEnumerable<ModelColumn> added, IEnumerable<ModelColumn> removed)
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
        private void OnMultiSelectedItemChanged(ModelColumn item, bool remove)
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
        private void OnSelectedAreaChanged(Rect? arg1, bool delete)
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

        #region Событие начала изменения ширины
        /// <summary>
        /// Событие начала изменения ширины
        /// </summary>
        /// <param name="item"></param>
        private void OnDragStartedEvent(ModelColumn item) => TableFactory?
            .UpdateLayoutDrag(Orientation.Vertical, item.PositionX, item.Right);
        #endregion

        #region Событие изменения ширины
        /// <summary>
        /// Событие изменения ширины
        /// </summary>
        /// <param name="delta"></param>
        private void OnWidthChangeEvent(ModelColumn item, double delta) => TableFactory?
            .UpdateLayoutDrag(Orientation.Vertical, item.PositionX, item.Right += delta);
        #endregion

        #region Событие завершения изменения ширины
        /// <summary>
        /// Событие завершения изменения ширины
        /// </summary>
        /// <param name="item"></param>
        private void OnDragCompletedEvent(ModelColumn item) => TableFactory?
            .UpdateLayoutDragComplete(Orientation.Vertical, item);
        #endregion

        #region Событие необходимости установки ширины по содержимому
        /// <summary>
        /// Событие необходимости установки ширины по содержимому
        /// </summary>
        /// <param name="item"></param>
        private void OnSizeToContentEvent(ModelColumn item)
        {
            //Debug.WriteLine(item.Header);
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
