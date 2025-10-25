using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.Views;
using AvaloniaTemplate.Views.UserDialogWindows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace AvaloniaTemplate.Services
{
    public class UserDialogService : IUserDialogService
    {
        private MainWindow pMainWindow;

        #region Удалить файл
        public bool DeleteFile(string SelectedFile)
        {
            bool result = false;

            if (File.Exists(SelectedFile))
            {
                File.Delete(SelectedFile);
                result = true;
            }
            return result;
        }
        #endregion

        #region Загрузить данные
        public T Load<T>(string path)
        {
            var settingsAppSerializer = new XmlSerializer(typeof(T));
            T result = default;
            try
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                result = (T)settingsAppSerializer.Deserialize(fs);

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Не удалось загрузить данные. Описание ошибки: " + e.Message);
            }
            return result;
        }
        #endregion

        #region Сохранить данные
        public bool Save<T>(T content, string path)
        {
            bool result = false;
            var SettingsAppSerializer = new XmlSerializer(typeof(T));

            try
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                SettingsAppSerializer.Serialize(fs, content);

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Не удалось сохранить данные. Описание ошибки: " + e.Message);
            }
            return result;
        }
        #endregion

        #region Метод открытия диалогового окна выбора файла
        public string SelectFile(string Title,
            string DefaultPath = null,
            bool AllowMultiple = false,
            FilePickerFileType Filter = null,
            Window Owner = null,
            IStorageProvider provider = default)
        {
            return FileDialogWindow.SelectFile(new OpenFileDialogOptionType
            {
                Title = Title,
                Owner = Owner,
                AllowMultiple = AllowMultiple,
                FileTypeFilter = Filter,
                SuggestedFileName = "",
                SuggestedStartLocation = DefaultPath
            }, provider);
        }
        #endregion

        #region Метод открытия диалогового окна выбора пути
        public string SelectFolder(string Title,
            string DefaultPath = null,
            Window Owner = null,
            IStorageProvider provider = default)
        {
            return FileDialogWindow.SelectFolder(new OpenFileDialogOptionType
            {
                Title = Title,
                Owner = Owner,
                SuggestedFileName = "",
                SuggestedStartLocation = DefaultPath
            }, provider);
        }
        #endregion

        #region Получить экземпляр главного окна
        public MainWindow GetMainWindow()
        {
            OpenMainWindow();
            return pMainWindow;
        }
        #endregion

        #region Открыть главное окно приложения
        public void OpenMainWindow()
        {
            if (pMainWindow is { } window) { window.Show(); return; }
            window = App.Services.GetRequiredService<MainWindow>();
            window.Closing += (s, e) => CloseMainWindow(e);
            window.Opened += (s, e) => OpennedMainWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            pMainWindow = window;
            window.Show();
        }
        #endregion

        #region Событие закрытия главного окна
        public void CloseMainWindow(WindowClosingEventArgs e)
        {
            var findResource = App.Current.FindResource("RequestConfirmCloseBeforeClosing");
            if (pMainWindow is null || findResource is null || findResource is not bool requestConfirm) return;

            var msg = "Вы действительно хотите выйти?";
            if (requestConfirm && !SendMessage("Внимание!", msg, pMainWindow, MessageBoxButtonType.YesNo, MessageBoxImageType.Warning, MessageBoxResultType.Yes))
            {
                e.Cancel = true;
                return;
            }
        }
        #endregion

        #region Событие завершения открытия главного окна
        public void OpennedMainWindow()
        {

        }
        #endregion

        #region Отправка сообщений пользователю
        public bool SendMessage(
            string Title,
            string Message,
            Window ownerWindow,
            MessageBoxButtonType ButtonType = MessageBoxButtonType.OK,
            MessageBoxImageType ImageType = MessageBoxImageType.Information,
            MessageBoxResultType ResultType = MessageBoxResultType.OK)
        => MessageBox.Show(Title, Message, ButtonType, ImageType, ResultType, ownerWindow) == ResultType; 
        #endregion
    }
}
