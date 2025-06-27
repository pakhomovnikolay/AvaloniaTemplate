using Avalonia;
using Avalonia.Platform.Storage;

namespace AvaloniaTemplate.Models.Enums.FileDialogOptionTypes
{
    public class OpenFileDialogOption
    {
        /// <summary>
        /// Заголовк окна
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Разрешить множественный выбор
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Фильтр расширений файлов
        /// </summary>
        public FilePickerFileType FileTypeFilter { get; set; }

        /// <summary>
        /// Имя файла по умолчанию
        /// </summary>
        public string SuggestedFileName { get; set; }

        /// <summary>
        /// Путь по умолчанию
        /// </summary>
        public string SuggestedStartLocation { get; set; }

        /// <summary>
        /// Главное окно
        /// </summary>
        public Visual Owner { get; set; }
    }
}
