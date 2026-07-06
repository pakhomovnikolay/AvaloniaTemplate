using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaTemplate.Services
{
    public class HorizontalTabStripService<T> : ObservableObject, IHorizontalTabStripService<T>
    {
        #region Функция вызываемая для создания нового элемента коллекции
        /// <summary>
        /// Функция вызываемая для создания нового элемента коллекции
        /// </summary>
        public Func<T> CreateItem { get; set; }
        #endregion

        #region Коллекция элементов
        private ObservableCollection<T> items;
        /// <summary>
        /// Коллекция элементов
        /// </summary>
        public ObservableCollection<T> ItemsSource
        {
            get => items;
            set => SetProperty(ref items, value);
        }
        #endregion

        #region Выбранный элемент коллекции
        private T selectedItem;
        /// <summary>
        /// Выбранный элемент коллекции
        /// </summary>
        public T SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }
        #endregion

        #region Команда - создать элемент для коллекции
        private ICommand command_CreateItem;
        /// <summary>
        /// Команда - создать элемент для коллекции
        /// </summary>
        public ICommand Command_CreateItem
        {
            get => command_CreateItem ??= new RelayCommand(ExecuteCommand_CreateItem, CanExecuteCommand_CreateItem);
            set => SetProperty(ref command_CreateItem, value);
        }
        private bool CanExecuteCommand_CreateItem(object p)
            => p is { } && p is ObservableCollection<T>;

        private void ExecuteCommand_CreateItem(object p)
        {
            if (!CanExecuteCommand_CreateItem(p))
                return;

            ItemsSource.Add(CreateItem());
            SelectedItem = Helper.GetSelectedElement<T>(ItemsSource.Count, ItemsSource);
        }
        #endregion

        #region Удалить выбранный элемент из коллекции
        /// <summary>
        /// Удалить выбранный элемент из коллекции
        /// </summary>
        public void DeleteSelectedItem()
        {
            if (SelectedItem is not { })
                return;

            DeleteSelectedItem(SelectedItem);
        }

        /// <summary>
        /// Удалить выбранный элемент из коллекции
        /// </summary>
        public void DeleteSelectedItem(T item)
        {
            if (item is not { })
                return;

            var index = ItemsSource.IndexOf(item);
            ItemsSource.Remove(item);
            SelectedItem = Helper.GetSelectedElement<T>(index, ItemsSource);
        }
        #endregion
    }
}
