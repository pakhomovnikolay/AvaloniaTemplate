using AvaloniaTemplate.Models.Table.Base;

namespace AvaloniaTemplate.Models.Table.Model
{
    public class ModelColumn : ModelBase<ModelColumn>
    {
        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelColumn()
        {
            Owner = this;
        }
        #endregion

        #region Индекс
        private int index;
        /// <summary>
        /// Индекс
        /// </summary>
        public int Index
        {
            get => index;
            set => SetProperty(ref index, value);
        }
        #endregion

        #region Заголовок
        private string header;
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }
        #endregion
    }
}