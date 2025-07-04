using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaTemplate.Models.Enums.MessageTypes;
using AvaloniaTemplate.Views;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IUserDialogService
    {
        #region Отправка сообщений пользователю
        /// <summary>
        /// Отправка сообщений пользователю
        /// </summary>
        /// <param name="Title">Заголовок окна</param>
        /// <param name="Message">Сообщение</param> 
        /// <param name="ownerWindow">Родительское окно</param>
        /// <param name="ButtonType">Конфигурация кнопок</param
        /// <param name="ImageType">Конфигурация изображения</param>
        /// <param name="ResultType">Требуемый результат события от пользователя</param>
        /// <returns></returns>
        bool SendMessage(string Title, string Message, Window ownerWindow,
            MessageBoxButtonType ButtonType = MessageBoxButtonType.OK,
            MessageBoxImageType ImageType = MessageBoxImageType.Information,
            MessageBoxResultType ResultType = MessageBoxResultType.OK
            );
        #endregion

        #region Метод открытия диалогового окна выбора файла
        /// <summary>
        /// Метод открытия диалогового окна выбора файла
        /// </summary>
        /// <param name="Title">Заголовок окна</param>
        /// <param name="DefaultPath">Путь к файлу по умолчанию</param>
        /// <param name="AllowMultiple">Разрешить множественный выбор</param>
        /// <param name="Filter">Фильтр расширения файлов</param>
        /// <returns></returns>
        string SelectFile(string Title,
            string DefaultPath = null,
            bool AllowMultiple = false,
            FilePickerFileType Filter = null,
            Window Owner = null,
            IStorageProvider provider = default);
        #endregion

        #region Метод открытия диалогового окна выбора пути
        /// <summary>
        /// Метод открытия диалогового окна выбора пути
        /// </summary>
        /// <param name="Title">Заголовок окна</param>
        /// <param name="DefaultPath">Путь по умолчанию</param>
        /// <returns></returns>
        string SelectFolder(string Title,
            string DefaultPath = null,
            Window Owner = null,
            IStorageProvider provider = default);
        #endregion

        #region Удалить файл
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="SelectedFile">Путь к файлу по умолчанию</param>
        /// <returns></returns>
        bool DeleteFile(string SelectedFile);
        #endregion

        #region Сохранить данные
        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="content">Данные для сохранения</param>
        /// <param name="path">Путь к файлу</param>
        bool Save<T>(T content, string path);
        #endregion

        #region Загрузить данные
        /// <summary>
        /// Загрузить данные
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        T Load<T>(string path);
        #endregion

        #region Открыть главное окно приложения
        /// <summary>
        /// Открыть главное окно приложения
        /// </summary>
        /// <returns></returns>
        void OpenMainWindow();
        #endregion

        #region Событие закрытия главного окна
        /// <summary>
        /// Событие закрытия главного окна
        /// </summary>
        /// <param name="e"></param>
        void CloseMainWindow(WindowClosingEventArgs e);
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
        MainWindow GetMainWindow();
        #endregion
    }
}
