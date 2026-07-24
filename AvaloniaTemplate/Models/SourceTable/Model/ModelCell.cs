using AvaloniaTemplate.Models.SourceTable.Base;

namespace AvaloniaTemplate.Models.SourceTable.Model
{
    public class ModelCell : ModelBase<ModelCell>
    {
        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelCell()
        {
            Owner = this;
        }
        #endregion

        #region Индекс колонки
        private int columnIndex;
        /// <summary>
        /// Индекс колонки
        /// </summary>
        public int ColumnIndex
        {
            get => columnIndex;
            set => SetProperty(ref columnIndex, value);
        }
        #endregion

        #region Индекс строки
        private int rowIndex;
        /// <summary>
        /// Индекс строки
        /// </summary>
        public int RowIndex
        {
            get => rowIndex;
            set => SetProperty(ref rowIndex, value);
        }
        #endregion

        #region Редактирование
        private bool isEdit;
        /// <summary>
        /// Редактирование
        /// </summary>
        public bool IsEdit
        {
            get => isEdit;
            set => SetProperty(ref isEdit, value);
        }
        #endregion

        #region Данные
        private string rawValue;
        /// <summary>
        /// Данные
        /// </summary>
        public string RawValue
        {
            get => rawValue;
            set => SetProperty(rawValue, value, v => { rawValue = v; ControlInpitRawValue(); });
        }
        #endregion

        #region Данные для отображения
        private string visualValue;
        /// <summary>
        /// Данные для отображения
        /// </summary>
        public string VisualValue
        {
            get => visualValue;
            set => SetProperty(ref visualValue, value);
        }
        #endregion

        #region Сбросить статусы
        /// <summary>
        /// Сбросить статусы
        /// </summary>
        public void ResetStatus()
        {
            IsSelected = false;
            IsFocused = false;
            IsHeader = false;
            IsEdit = false;
        }
        #endregion

        #region Контроль ввода данных
        /// <summary>
        /// Контроль ввода данных
        /// </summary>
        private void ControlInpitRawValue()
        {
            VisualValue = RawValue;
        } 
        #endregion
    }
}