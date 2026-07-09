using AvaloniaTemplate.Models.Table.Base;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.Models.Table.Model
{
    public class ModelRow : ModelBase<ModelRow>
    {
        #region Конструктор класса
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ModelRow()
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

        #region Коллекция ячеек
        private ObservableCollection<ModelCell> cells = [];
        /// <summary>
        /// Коллекция ячеек
        /// </summary>
        public ObservableCollection<ModelCell> Cells
        {
            get => cells;
            set => SetProperty(ref cells, value);
        }
        #endregion

        #region Выбранная ячейка
        private ModelCell selectedModelCell;
        /// <summary>
        /// Выбранная ячейка
        /// </summary>
        public ModelCell SelectedModelCell
        {
            get => selectedModelCell;
            set => SetProperty(ref selectedModelCell, value);
        }
        #endregion

        #region Выбранные ячейки
        private ObservableCollection<ModelCell> selectedModelCells = [];
        /// <summary>
        /// Выбранные ячейки
        /// </summary>
        public ObservableCollection<ModelCell> SelectedModelCells
        {
            get => selectedModelCells;
            set => SetProperty(ref selectedModelCells, value);
        }
        #endregion
    }
}