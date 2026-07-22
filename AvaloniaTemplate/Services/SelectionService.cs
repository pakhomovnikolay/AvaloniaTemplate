using Avalonia;
using Avalonia.Input;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.SourceTable.Base.Interfaces;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTemplate.Services
{
    public class SelectionService<T>(
        Func<IEnumerable<T>> _source,
        Func<T, int> _selector = default
        ) : ISelectionService<T> where T : IModelBase<T>
    {
        private readonly Func<IEnumerable<T>> source = _source;
        private readonly Func<T, int> selector = _selector;
        private readonly StatusSelected statusSelected = new();
        private readonly HashSet<T> selections = [];
        private HashSet<T> baseSelections = [];

        #region Событие изменения текущих элементов
        /// <summary>
        /// Событие изменения текущих элементов
        /// A: Добавить
        /// B:  Удалить
        /// </summary>
        public event Action<IEnumerable<T>, IEnumerable<T>>? SelectedChangedItems;
        #endregion

        #region Событие изменения текущего элемента
        /// <summary>
        /// Событие изменения текущего элемента
        /// T: Элемент
        /// </summary>
        public event Action<T>? SelectedChangedItem;
        #endregion

        #region Событие изменения выбранного элементов
        /// <summary>
        /// Событие изменения выбранного элементов
        /// T: Элемент
        /// bool: 1 - Добавить\ 2 - Удалить
        /// </summary>
        public event Action<T, bool>? MultiSelectedChangedItem;
        #endregion

        #region Событие изменения выделенной области
        /// <summary>
        /// Событие изменения выделенной области
        /// bool: 1 - Добавить\ 2 - Удалить
        /// </summary>
        public event Action<Rect?, bool>? SelectedAreaChanged;
        #endregion

        #region  Установить активный элемент
        /// <summary>
        /// Установить активный элемент
        /// </summary>
        /// <param name="item"></param>
        public void SetFocus(T item)
            => item.IsFocused = true;
        #endregion

        #region Сбросить активный элемент
        /// <summary>
        /// Сбросить активный элемент
        /// </summary>
        /// <param name="item"></param>
        public void ResetFocus(T item)
            => item.IsFocused = false;
        #endregion

        #region Установить текущий элемент
        /// <summary>
        /// Установить текущий элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        public void SetSelected(PointerPressedEventArgs e, T item)
        {
            if (!e.Properties.IsLeftButtonPressed || item is not { })
                return;

            statusSelected.IsMoved = false;
            statusSelected.IsPressed = true;
            statusSelected.IsCtrl = e.KeyModifiers == KeyModifiers.Control;
            statusSelected.IsShift = e.KeyModifiers == KeyModifiers.Shift;
            statusSelected.IsWasSelected = IsWasSelected(item) && statusSelected.IsCtrl;

            if (!statusSelected.IsShift)
            {
                if (!statusSelected.IsCtrl)
                    SetSingleItem(item);
                else
                    UpdateMultiSelects(item);

                baseSelections = source().Where(x => x.IsSelected)?.ToHashSet();
                selections.Clear();
                if (!statusSelected.IsWasSelected)
                    statusSelected.StartIndex = statusSelected.CurrentIndex;
            }
            else
            {
                var array = GetRange(statusSelected.StartIndex, statusSelected.CurrentIndex);
                ApplyRange(array);
            }
        }
        #endregion

        #region Выбрать диапазон текущих элементов
        /// <summary>
        /// Выбрать диапазон текущих элементов
        /// </summary>
        /// <param name="index"></param>
        public void SetRangeSelected(int index)
        {
            statusSelected.IsMoved = true;
            var startIndex = statusSelected.IsShift
                ? statusSelected.StartIndex
                : statusSelected.CurrentIndex;

            var range = GetRange(startIndex, index);
            ApplyRange(range);
        }
        #endregion

        #region Получить состояние нажатой клавиши Ctrl
        /// <summary>
        /// Получить состояние нажатой клавиши Ctrl
        /// </summary>
        /// <returns></returns>
        public bool GetIsCtrl()
            => statusSelected.IsCtrl;
        #endregion

        #region Получить состояние нажатой клавиши Shift
        /// <summary>
        /// Получить состояние нажатой клавиши Shift
        /// </summary>
        /// <returns></returns>
        public bool GetIsShift()
            => statusSelected.IsShift;
        #endregion

        #region Получить состояние выбора диапазона элементов
        /// <summary>
        /// Получить состояние выбора диапазона элементов
        /// </summary>
        /// <returns></returns>
        public bool GetIsMoved()
            => statusSelected.IsMoved;
        #endregion

        #region Проверка на пересечения
        /// <summary>
        /// Проверка на пересечения
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Intersects(T c, Rect r)
            => c.Geometry.Bounds.Intersects(r);
        #endregion

        #region Построить область выделения
        /// <summary>
        /// Построить область выделения
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public Rect BuildBoundingRect(IEnumerable<T> cells)
        {
            if (cells is not { } || !cells.Any())
                return new();

            var left = cells.Min(c => c.Geometry.PositionX);
            var top = cells.Min(c => c.Geometry.PositionY);
            var right = cells.Max(c => c.Geometry.Right);
            var bottom = cells.Max(c => c.Geometry.Bottom);

            return new Rect(left, top, right - left, bottom - top);
        }
        #endregion

        #region private

        #region Установить одиночный элемент
        /// <summary>
        /// Установить одиночный элемент
        /// </summary>
        /// <param name="item"></param>
        private void SetSingleItem(T item)
            => UpdateSingleSelect(item);
        #endregion

        #region Применить диапазон
        /// <summary>
        /// Применить диапазон
        /// </summary>
        private void ApplyRange(IEnumerable<T> array)
            => UpdateRangeSelect(array);
        #endregion

        #region Получить диапазон
        /// <summary>
        /// Получить диапазон
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private IEnumerable<T> GetRange(int startIndex, int endIndex)
        {
            var indexMin = Math.Min(startIndex, endIndex);
            var indexMax = Math.Max(startIndex, endIndex);
            return source().Where(x =>
            {
                var index = selector(x);
                return index >= indexMin && index <= indexMax;
            });
        }
        #endregion

        #region Выбранный
        /// <summary>
        /// Выбранный
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsWasSelected(T item)
        {
            statusSelected.CurrentIndex = selector(item);
            return source().FirstOrDefault(x => x.IsSelected && x.Equals(item)) is { };
        }
        #endregion

        #region Обновить выбранный жлемент
        /// <summary>
        /// Обновить выбранный элемент
        /// </summary>
        /// <param name="item"></param>
        private void UpdateSingleSelect(T item)
        {
            SelectedChangedItem?.Invoke(item);
            SelectedChangedItems?.Invoke([item], []);
            UpdateSelectedArea([item], statusSelected.IsWasSelected);
        }
        #endregion

        #region Обновить выбранные элементы
        /// <summary>
        /// Обновить выбранные элементы
        /// </summary>
        /// <param name="item"></param>
        private void UpdateMultiSelects(T item)
        {
            MultiSelectedChangedItem?.Invoke(item, statusSelected.IsWasSelected);
            if (statusSelected.IsWasSelected)
                SelectedChangedItems?.Invoke([], [item]);
            else
                SelectedChangedItems?.Invoke([item], []);
            UpdateSelectedArea([item], statusSelected.IsWasSelected);
        }
        #endregion

        #region Обновить диапазон выбранных элементов
        /// <summary>
        /// Обновить диапазон выбранных элементов
        /// </summary>
        /// <param name="array"></param>
        private void UpdateRangeSelect(IEnumerable<T> array)
        {
            var current = array.ToHashSet();

            HashSet<T> newSelection = [.. baseSelections];
            if (statusSelected.IsWasSelected)
                newSelection.ExceptWith(current);
            else
                newSelection.UnionWith(current);

            // считаем разницу относительно ПРЕДЫДУЩЕГО итогового состояния
            var add = newSelection.Except(source().Where(x => x.IsSelected)?.ToHashSet()).ToList();
            var remove = source().Where(x => x.IsSelected)?.ToHashSet().Except(newSelection).ToList();

            // обновляем lastRange
            selections.Clear();
            foreach (var item in current)
                selections.Add(item);

            SelectedChangedItems?.Invoke(add, remove);
            UpdateSelectedArea(selections, statusSelected.IsWasSelected);
        }
        #endregion

        #region Обновить выделенную область
        /// <summary>
        /// Обновить выделенную область
        /// </summary>
        /// <param name="items"></param>
        /// <param name="delete"></param>
        private void UpdateSelectedArea(IEnumerable<T> items, bool delete = false)
        {
            var rect = BuildBoundingRect(items);
            while (true)
            {
                var selected = source()
                    .Where(c => Intersects(c, rect))
                    .ToList();

                var newRect = BuildBoundingRect(selected);
                if (RectsEqual(rect, newRect) || selected.Count <= 0)
                    break;

                rect = newRect;
            }
            SelectedAreaChanged?.Invoke(rect, delete);
        }
        #endregion

        #region Сравнить область выделения с текущей
        /// <summary>
        /// Сравнить область выделения с текущей
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool RectsEqual(Rect a, Rect b)
        {
            return Math.Abs(a.X - b.X) < 0.1 &&
                   Math.Abs(a.Y - b.Y) < 0.1 &&
                   Math.Abs(a.Width - b.Width) < 0.1 &&
                   Math.Abs(a.Height - b.Height) < 0.1;
        }
        #endregion

        #endregion
    }
}
