using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Enums.FileDialogOptionTypes;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IUserDialogService
    {
        #region Отправка сообщения пользователю
        /// <summary>
        /// Отправка сообщения пользователю
        /// </summary>
        /// <param name="title"> Заголовок окна </param>
        /// <param name="message"> Сообщение </param>
        /// <param name="owner"> Родительское окно </param>
        /// <param name="buttonType"> Конфигурация кнопок </param>
        /// <param name="imageType"> Конфигурация изображения </param>
        /// <param name="resultType"> Требуемый результат события от пользователя </param>
        /// <returns></returns>
        Task<MessageBoxResultType> SendMessageAsync(string title, string message, Window owner,
            MessageBoxButtonType buttonType = MessageBoxButtonType.OK,
            MessageBoxImageType imageType = MessageBoxImageType.Information,
            MessageBoxResultType resultType = MessageBoxResultType.OK
            );
        #endregion

        #region Открыть диалоговое окно выбора файла
        /// <summary>
        /// Открыть диалоговое окно выбора файла
        /// </summary>
        /// <param name="title"> Заголовок окна </param>
        /// <param name="dialogType"> Тип диалогового окна </param>
        /// <param name="allowMultiple"> Разрешить множественный выбор </param>
        /// <param name="defaultPath"> Путь к файлу по умолчанию </param>
        /// <param name="filter"> Фильтр расширения файлов </param>
        /// <param name="owner"> Родительское окно </param>
        /// <param name="provider"> Провайдер данных </param>
        /// <returns></returns>
        Task<string> SelectFileAsync(string title,
            FileDialogType dialogType = FileDialogType.Open,
            bool allowMultiple = false,
            string defaultPath = null,
            FilePickerFileType filter = null,
            Window owner = null,
            IStorageProvider provider = default);
        #endregion

        #region Открыть диалоговое окно выбора пути
        /// <summary>
        /// Открыть диалоговое окно выбора пути
        /// </summary>
        /// <param name="title"> Заголовок окна </param>
        /// <param name="defaultPath"> Путь по умолчанию </param>
        /// <param name="provider"> Провайдер данных </param>
        /// <returns></returns>
        Task<string> SelectFolderAsync(string title, string defaultPath = null, IStorageProvider provider = default);
        #endregion

        #region Удалить файл
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <exception cref="FileNotFoundException">Файл по указаннму пути не найден</exception>
        /// <exception cref="IOException">Нет доступа к файлу или каталогу</exception>
        /// <returns>DialogResult</returns>
        Task<DialogResult> DeleteFileAsync(string selectedFile);
        #endregion

        #region Сохранить данные
        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        /// <exception cref="FileNotFoundException">Файл по указаннму пути не найден</exception>
        /// <exception cref="IOException">Нет доступа к файлу или каталогу</exception>
        /// <returns>DialogResult</returns>
        Task<DialogResult> SaveAsync<T>(T content, string filePath);
        #endregion

        #region Загрузить данные
        /// <summary>
        /// Загрузить данные
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <exception cref="FileNotFoundException">Файл по указаннму пути не найден</exception>
        /// <exception cref="IOException">Нет доступа к файлу или каталогу</exception>
        /// <returns>DialogResult</returns>
        Task<T> LoadAsync<T>(string filePath);
        #endregion

        #region Событие закрытия главного окна
        /// <summary>
        /// Событие закрытия главного окна
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task CloseMainWindowAsync(WindowClosingEventArgs e);
        #endregion

        #region Событие завершения открытия главного окна
        /// <summary>
        /// Событие завершения открытия главного окна
        /// </summary>
        void OpennedMainWindow();
        #endregion

        #region Получить экземпляр главного окна
        /// <summary>
        /// Получить экземпляр главного окна
        /// </summary>
        /// <returns></returns>
        Window GetMainWindow();
        #endregion
    }
}
