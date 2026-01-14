using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.Views;
using AvaloniaTemplate.Views.UserDialogWindows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvaloniaTemplate.Services
{
    public class UserDialogService : IUserDialogService
    {
        #region Отправка сообщения пользователю
        public async Task<MessageBoxResultType> SendMessageAsync(string title,
            string message,
            Window owner,
            MessageBoxButtonType buttonType = MessageBoxButtonType.OK,
            MessageBoxImageType imageType = MessageBoxImageType.Information,
            MessageBoxResultType resultType = MessageBoxResultType.OK)
            => await MessageBox.ShowAsync(new OpenMessageDialogOptionType
            {
                Title = title,
                Message = message,
                ButtonType = buttonType,
                ImageType = imageType,
                ResultType = resultType
            }, owner ?? App.Desktop.MainWindow);
        #endregion

        #region Открыть диалоговое окно выбора файла
        public async Task<string> SelectFile(string title,
            FileDialogType dialogType = FileDialogType.Open,
            bool allowMultiple = false, string defaultPath = null,
            FilePickerFileType filter = null, Window owner = null,
            IStorageProvider provider = default)
            => dialogType == FileDialogType.Save

            ? await FileDialogWindow.SelectFileAsSaveAsync(new OpenFileDialogOptionType
            {
                Title = string.IsNullOrWhiteSpace(title) ? App.Desktop.MainWindow.Title : title,
                AllowMultiple = allowMultiple,
                FileTypeFilter = filter ?? FilePickerFileTypes.All,
                SuggestedFileName = "",
                SuggestedStartLocation = defaultPath ?? App.FolderPath
            }, provider ?? App.GetTopLevel()?.StorageProvider)

            : await FileDialogWindow.SelectFileAsync(new OpenFileDialogOptionType
            {
                Title = string.IsNullOrWhiteSpace(title) ? App.Desktop.MainWindow.Title : title,
                AllowMultiple = allowMultiple,
                FileTypeFilter = filter ?? FilePickerFileTypes.All,
                SuggestedFileName = "",
                SuggestedStartLocation = defaultPath ?? App.FolderPath
            }, provider ?? App.GetTopLevel()?.StorageProvider);
        #endregion

        #region Открыть диалоговое окно выбора пути
        /// <summary>
        /// Открыть диалоговое окно выбора пути
        /// </summary>
        /// <param name="title"> Заголовок окна </param>
        /// <param name="defaultPath"> Путь по умолчанию </param>
        /// <param name="provider"> Провайдер данных </param>
        /// <returns></returns>
        public async Task<string> SelectFolder(string title,
            string defaultPath = null,
            IStorageProvider provider = default)
            => await FileDialogWindow.SelectFolder(new OpenFileDialogOptionType
            {
                Title = string.IsNullOrWhiteSpace(title) ? App.Desktop.MainWindow.Title : title,
                SuggestedFileName = "",
                SuggestedStartLocation = defaultPath ?? App.FolderPath
            }, provider ?? App.GetTopLevel()?.StorageProvider);
        #endregion

        #region Удалить файл
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="selectedFile"> Путь к файлу по умолчанию </param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string selectedFile)
        {
            var result = false;
            if (!File.Exists(selectedFile))
                await SendMessageAsync("Удаление файла",
                    "Не удается найти указанный файл. Проверьте путь",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);
            else
            {
                try
                {
                    File.Delete(selectedFile);
                    result = true;
                }
                catch (Exception e)
                {
                    await SendMessageAsync("Удаление файла",
                        $"В процессе удаления файла произошла ошибка: {e}",
                        App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);
                }
            }
            return result;
        }
        #endregion

        #region Сохранить данные
        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<bool> Save<T>(T content, string path)
        {
            var result = false;
            var SettingsAppSerializer = new XmlSerializer(typeof(T));

            try
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                SettingsAppSerializer.Serialize(fs, content);
                result = true;
            }
            catch (Exception e)
            {
                await SendMessageAsync("Сохранение данных",
                    $"В процессе сохранения данных произошла ошибка: {e}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);
            }
            return result;
        }
        #endregion

        #region Загрузить данные
        /// <summary>
        /// Загрузить данные
        /// </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <returns></returns>
        public async Task<T> Load<T>(string path)
        {
            T result = default;
            var settingsAppSerializer = new XmlSerializer(typeof(T));

            try
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                result = (T)settingsAppSerializer.Deserialize(fs);
            }
            catch (Exception e)
            {
                await SendMessageAsync("Загрузка данных",
                    $"В процессе загрузки данных произошла ошибка: {e}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);
            }
            return result;
        }
        #endregion

        #region Событие закрытия главного окна
        /// <summary>
        /// Событие закрытия главного окна
        /// </summary>
        /// <param name="e"></param>
        public async Task CloseMainWindow(WindowClosingEventArgs e)
        {
            var requestConfirm = App.GetStateRequestConfirmCloseBeforeClosing();
            if (App.Desktop?.MainWindow is not { } window || !requestConfirm)
                return;

            e.Cancel = true;
            if (App.GetStatusAppDataChanged())
            {
                var msg = "В приложение были внесены изменения\nВы хотите сохранить изменения?";
                var result = await SendMessageAsync("Внимание!", msg, window,
                    MessageBoxButtonType.YesNoCancel,
                    MessageBoxImageType.Question,
                    MessageBoxResultType.Cancel);

                if (result != MessageBoxResultType.Cancel)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        // Устанавливаем флаг, чтобы избежать повторного запроса
                        App.ChangeStatusConfirmCloseBeforeClosing(false);

                        // Закрываем окно (это не вызовет повторного WindowClosing)
                        window.Close();
                    });
                }
            }
            else
            {
                var msg = "Вы действительно хотите выйти?";
                var result = await SendMessageAsync("Внимание!", msg, window,
                    MessageBoxButtonType.YesNo,
                    MessageBoxImageType.Question,
                    MessageBoxResultType.Yes);

                if (result == MessageBoxResultType.Yes)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        // Устанавливаем флаг, чтобы избежать повторного запроса
                        App.ChangeStatusConfirmCloseBeforeClosing(false);

                        // Закрываем окно (это не вызовет повторного WindowClosing)
                        window.Close();
                    });
                }
            }
        }
        #endregion

        #region Событие завершения открытия главного окна
        /// <summary>
        /// Событие завершения открытия главного окна
        /// </summary>
        public void OpennedMainWindow() { }
        #endregion

        #region Получить экземпляр главного окна
        /// <summary>
        /// Получить экземпляр главного окна
        /// </summary>
        /// <returns></returns>
        public Window GetMainWindow()
            => App.Desktop.MainWindow is null
            ? OpenMainWindow()
            : App.Desktop?.MainWindow;
        #endregion

        #region Открыть главное окно приложения
        /// <summary>
        /// Открыть главное окно приложения
        /// </summary>
        /// <returns></returns>
        private Window OpenMainWindow()
        {
            if (App.Desktop?.MainWindow is { } window) { window.Show(); return window; }

            window = App.Services.GetRequiredService<MainWindow>();
            window.Closing += async (s, e) => await CloseMainWindow(e);
            window.Opened += (s, e) => OpennedMainWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
            return window;
        }
        #endregion
    }
}
