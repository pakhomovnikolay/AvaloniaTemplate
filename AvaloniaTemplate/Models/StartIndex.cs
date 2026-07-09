namespace AvaloniaTemplate.Models
{
    public class StartIndex
    {
        #region Стартовый индекс колонки
        /// <summary>
        /// Стартовый индекс колонки
        /// </summary>
        public int Column { get; set; }
        #endregion

        #region Стартовый индекс строки
        /// <summary>
        /// Стартовый индекс строки
        /// </summary>
        public int Row { get; set; }
        #endregion

        #region Текущий индекс колонки
        /// <summary>
        /// Текущий индекс колонки
        /// </summary>
        public int CurrentColumn { get; private set; }
        #endregion

        #region Текущий индекс строки
        /// <summary>
        /// Текущий индекс строки
        /// </summary>
        public int CurrentRow { get; private set; }
        #endregion

        #region Различие колонок
        /// <summary>
        /// Различие колонок
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsEqaulColumn(int index)
        {
            if (CurrentColumn == index)
                return true;

            CurrentColumn = index;
            return false;
        }
        #endregion

        #region Различие строк
        /// <summary>
        /// Различие строк
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsEqaulRow(int index)
        {
            if (CurrentRow == index)
                return true;

            CurrentRow = index;
            return false;
        }
        #endregion
    }
}
