namespace AvaloniaTemplate.Models
{
    public class StatusSelected
    {
        #region Текущий индекс
        /// <summary>
        /// Текущий индекс
        /// </summary>
        public int CurrentIndex { get; set; }
        #endregion

        #region Текущий индекс
        /// <summary>
        /// Текущий индекс
        /// </summary>
        public (int col, int row) CurrentCell { get; set; }
        #endregion

        #region Стартовый индекс
        /// <summary>
        /// Стартовый индекс
        /// </summary>
        public int StartIndex { get; set; }
        #endregion

        #region Стартовый индекс
        /// <summary>
        /// Стартовый индекс
        /// </summary>
        public (int col, int row) StartCell { get; set; }
        #endregion

        #region Указатель нажат
        /// <summary>
        /// Указатель нажат
        /// </summary>
        public bool IsPressed { get; set; }
        #endregion

        #region Указатель в движении
        /// <summary>
        /// Указатель в движении
        /// </summary>
        public bool IsMoved { get; set; }
        #endregion

        #region Стартовый элемент уже был выбран
        /// <summary>
        /// Стартовый элемент уже был выбран
        /// </summary>
        public bool IsWasSelected { get; set; }
        #endregion

        #region Нажата клавиша Ctrl
        /// <summary>
        /// Нажата клавиша Ctrl
        /// </summary>
        public bool IsCtrl { get; set; }
        #endregion

        #region Нажата клавиша Shift
        /// <summary>
        /// Нажата клавиша Shift
        /// </summary>
        public bool IsShift { get; set; }
        #endregion
    }
}
