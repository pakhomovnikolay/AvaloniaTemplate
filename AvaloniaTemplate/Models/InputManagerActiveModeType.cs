namespace AvaloniaTemplate.Models
{
    public enum AppActiveModeType
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown,

        /// <summary>
        /// Режим редактирования ячейки
        /// </summary>
        IsEditCell,

        /// <summary>
        /// Режим редактирования приложения
        /// </summary>
        IsEditApp,

        /// <summary>
        /// Режим навигации
        /// </summary>
        Navigation,

        /// <summary>
        /// Режим ввода
        /// </summary>
        IsInput,

        /// <summary>
        /// Режим взаимодействия с бефером обмена
        /// </summary>
        Clipboard
    }
}
