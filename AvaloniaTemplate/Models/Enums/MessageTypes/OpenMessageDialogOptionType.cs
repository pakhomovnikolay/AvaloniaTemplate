namespace AvaloniaTemplate.Models.Enums.MessageTypes
{
    public class OpenMessageDialogOptionType
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Конфигурация кнопок
        /// </summary>
        public MessageBoxButtonType ButtonType { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageBoxImageType ImageType { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public MessageBoxResultType ResultType { get; set; }
    }
}
