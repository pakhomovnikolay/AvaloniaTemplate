using Avalonia;
using Avalonia.Input;
using AvaloniaTemplate.Models.Table.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface ISelectionService<T> where T : IModelBase<T>
    {
        #region Событие изменения текущих элементов
        /// <summary>
        /// Событие изменения текущих элементов
        /// A: Добавить
        /// B:  Удалить
        /// </summary>
        event Action<IEnumerable<T>, IEnumerable<T>>? SelectedChangedItems;
        #endregion

        #region Событие изменения текущего элемента
        /// <summary>
        /// Событие изменения текущего элемента
        /// T: Элемент
        /// </summary>
        event Action<T>? SelectedChangedItem;
        #endregion

        #region Событие изменения выбранного элементов
        /// <summary>
        /// Событие изменения выбранного элементов
        /// T: Элемент
        /// bool: 1 - Добавить\ 2 - Удалить
        /// </summary>
        event Action<T, bool>? MultiSelectedChangedItem;
        #endregion

        #region Событие изменения выделенной области
        /// <summary>
        /// Событие изменения выделенной области
        /// bool: 1 - Добавить\ 2 - Удалить
        /// </summary>
        event Action<Rect?, bool>? SelectedAreaChanged;
        #endregion

        #region  Установить активный элемент
        /// <summary>
        /// Установить активный элемент
        /// </summary>
        /// <param name="item"></param>
        void SetFocus(T item);
        #endregion

        #region Сбросить активный элемент
        /// <summary>
        /// Сбросить активный элемент
        /// </summary>
        /// <param name="item"></param>
        void ResetFocus(T item);
        #endregion

        #region Установить текущий элемент
        /// <summary>
        /// Установить текущий элемент
        /// </summary>
        /// <param name="e"></param>
        /// <param name="item"></param>
        void SetSelected(PointerPressedEventArgs e, T item);
        #endregion

        #region Выбрать диапазон текущих элементов
        /// <summary>
        /// Выбрать диапазон текущих элементов
        /// </summary>
        /// <param name="index"></param>
        void SetRangeSelected(int index);
        #endregion

        #region Получить состояние нажатой клавиши Ctrl
        /// <summary>
        /// Получить состояние нажатой клавиши Ctrl
        /// </summary>
        /// <returns></returns>
        bool GetIsCtrl();
        #endregion

        #region Получить состояние нажатой клавиши Shift
        /// <summary>
        /// Получить состояние нажатой клавиши Shift
        /// </summary>
        /// <returns></returns>
        bool GetIsShift();
        #endregion

        #region Получить состояние выбора диапазона элементов
        /// <summary>
        /// Получить состояние выбора диапазона элементов
        /// </summary>
        /// <returns></returns>
        bool GetIsMoved();
        #endregion

        #region Проверка на пересечения
        /// <summary>
        /// Проверка на пересечения
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        bool Intersects(T c, Rect r);
        #endregion

        #region Построить область выделения
        /// <summary>
        /// Построить область выделения
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        Rect BuildBoundingRect(IEnumerable<T> cells);
        #endregion
    }
}
