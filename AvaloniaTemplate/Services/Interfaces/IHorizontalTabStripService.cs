using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IHorizontalTabStripService<T>
    {
        #region Функция вызываемая для создания нового элемента коллекции
        /// <summary>
        /// Функция вызываемая для создания нового элемента коллекции
        /// </summary>
        Func<T> CreateItem { get; set; } 
        #endregion

        #region Коллекция элементов
        /// <summary>
        /// Коллекция элементов
        /// </summary>
        ObservableCollection<T> ItemsSource { get; set; }
        #endregion

        #region Выбранный элемент коллекции
        /// <summary>
        /// Выбранный элемент коллекции
        /// </summary>
        T SelectedItem { get; set; }
        #endregion

        #region Команда - создать элемент для коллекции
        /// <summary>
        /// Команда - создать элемент для коллекции
        /// </summary>
        ICommand Command_CreateItem { get; set; }
        #endregion

        #region Удалить выбранный элемент из коллекции
        /// <summary>
        /// Удалить выбранный элемент из коллекции
        /// </summary>
        void DeleteSelectedItem();

        /// <summary>
        /// Удалить выбранный элемент из коллекции
        /// </summary>
        void DeleteSelectedItem(T item); 
        #endregion
    }
}