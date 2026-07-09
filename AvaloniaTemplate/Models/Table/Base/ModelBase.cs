using AvaloniaTemplate.Models.Table.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models.Table.Base
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

        #region Ширина
        private double width;
        /// <summary>
        /// Ширина
        /// </summary>
        public double Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }
        #endregion

        #region Высота
        private double height;
        /// <summary>
        /// Высота
        /// </summary>
        public double Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }
        #endregion

        #region Положение по оси X
        private double positionX;
        /// <summary>
        /// Положение по оси X
        /// </summary>
        public double PositionX
        {
            get => positionX;
            set => SetProperty(ref positionX, value);
        }
        #endregion

        #region Положение по оси Y
        private double positionY;
        /// <summary>
        /// Положение по оси Y
        /// </summary>
        public double PositionY
        {
            get => positionY;
            set => SetProperty(ref positionY, value);
        }
        #endregion

        #region Координата крайней точки по оси X
        private double right;
        /// <summary>
        /// Координата крайней точки по оси X
        /// </summary>
        public double Right
        {
            get => right;
            set => SetProperty(ref right, value);
        }
        #endregion

        #region Координата крайней точки по оси Y
        private double bottom;
        /// <summary>
        /// Координата крайней точки по оси Y
        /// </summary>
        public double Bottom
        {
            get => bottom;
            set => SetProperty(ref bottom, value);
        }
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