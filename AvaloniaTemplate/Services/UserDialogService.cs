using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.Views;
using AvaloniaTemplate.Views.UserDialogWindows;
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
        public async Task<string> SelectFileAsync(string title,
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
        public async Task<string> SelectFolderAsync(string title,
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
        public async Task<DialogResult> DeleteFileAsync(string selectedFile)
        {
            var result = DialogResult.Ok();
            try
            {
                if (!File.Exists(selectedFile))
                    throw new FileNotFoundException("Файл по указаннму пути не найден");

                File.Delete(selectedFile);
                return result;
            }
            catch (FileNotFoundException e)
            {
                await SendMessageAsync("Удаление файла",
                    $"Ошибка удаления файла.\n{e.Message}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                result = DialogResult.Fail(e.Message, e);
                return result;
            }
            catch (IOException e)
            {
                await SendMessageAsync("Удаление файла",
                    $"Ошибка удаления файла.\nНет доступа к файлу или каталогу",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                result = DialogResult.Fail("Нет доступа к файлу или каталогу", e);
                return result;
            }
        }
        #endregion

        #region Сохранить данные
        public async Task<DialogResult> SaveAsync<T>(T content, string filePath)
        {
            var result = DialogResult.Ok();
            var settingsAppSerializer = new XmlSerializer(typeof(T));

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Файл по указаннму пути не найден");

                await using FileStream fs = new(filePath, FileMode.OpenOrCreate);
                settingsAppSerializer.Serialize(fs, content);
                return result;
            }
            catch (FileNotFoundException e)
            {
                await SendMessageAsync("Сохранение файла",
                    $"Ошибка сохранения файла.\n{e.Message}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                result = DialogResult.Fail(e.Message, e);
                return result;
            }
            catch (IOException e)
            {
                await SendMessageAsync("Сохранение файла",
                    $"Ошибка сохранения файла.\nНет доступа к файлу или каталогу",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                result = DialogResult.Fail("Нет доступа к файлу или каталогу", e);
                return result;
            }
        }
        #endregion

        #region Загрузить данные
        public async Task<T> LoadAsync<T>(string filePath)
        {
            T result = default;
            var settingsAppSerializer = new XmlSerializer(typeof(T));

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Файл по указаннму пути не найден");

                await using FileStream fs = new(filePath, FileMode.OpenOrCreate);
                result = (T)settingsAppSerializer.Deserialize(fs);
                return result;
            }
            catch (FileNotFoundException e)
            {
                await SendMessageAsync("Загрузка данных",
                    $"Ошибка загрузки данных.\n{e.Message}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                return result;
            }
            catch (IOException e)
            {
                await SendMessageAsync("Загрузка данных",
                    $"В процессе загрузки данных произошла ошибка: {e}",
                    App.Desktop.MainWindow, imageType: MessageBoxImageType.Warning);

                return result;
            }
        }
        #endregion

        #region Событие закрытия главного окна
        /// <summary>
        /// Событие закрытия главного окна
        /// </summary>
        /// <param name="e"></param>
        public async Task CloseMainWindowAsync(WindowClosingEventArgs e)
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
                        App.ChangeConfirmCloseBeforeClosing(false);

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
                        App.ChangeConfirmCloseBeforeClosing(false);

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

            window = App.GetService<MainWindow>();
            window.Closing += async (s, e) => await CloseMainWindowAsync(e);
            window.Opened += (s, e) => OpennedMainWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
            return window;
        }
        #endregion
    }
}
