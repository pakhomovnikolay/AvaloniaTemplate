using AvaloniaTemplate.Models.SourceTable.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.SourceTable.Base
{
    public class ModelBase<T> : ObservableObject, IModelBase<T>
    {
        #region Идентификатор
        private string id;
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        #endregion

        #region Геометрия позиционирования
        /// <summary>
        /// Геометрия позиционирования
        /// </summary>
        public Geometry Geometry { get; init; } = new();
        #endregion

        #region Видимость
        private bool visible;
        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsVisible
        {
            get => visible;
            set => SetProperty(ref visible, value);
        }
        #endregion

        #region Выбрана
        private bool selected;
        /// <summary>
        /// Выбрана
        /// </summary>
        public virtual bool IsSelected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }
        #endregion

        #region Активная
        private bool focused;
        /// <summary>
        /// Активная
        /// </summary>
        public virtual bool IsFocused
        {
            get => focused;
            set => SetProperty(ref focused, value);
        }
        #endregion

        #region Стиль ячейки
        private ModelCellStyle cellStyle;
        /// <summary>
        /// Стиль ячейки
        /// </summary>
        public ModelCellStyle CellStyle
        {
            get => cellStyle;
            set => SetProperty(ref cellStyle, value);
        }
        #endregion

        #region Корневой элемент
        private T owner;
        /// <summary>
        /// Корневой элемент
        /// </summary>
        public T Owner
        {
            get => owner;
            set => SetProperty(ref owner, value);
        }
        #endregion
    }
}