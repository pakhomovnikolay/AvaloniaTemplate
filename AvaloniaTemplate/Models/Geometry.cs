using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Models
{
    public class Geometry : ObservableObject
    {
        #region Ширина
        private double width;
        /// <summary>
        /// Ширина
        /// </summary>
        public double Width
        {
            get => width;
            set => SetProperty(width, value, v
                =>
            { width = v; NotifyHorizontalGeometryChanged(); });
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
            set => SetProperty(height, value, v
                =>
            { height = v; NotifyVerticalGeometryChanged(); });
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
            set => SetProperty(positionX, value, v
                =>
            { positionX = v; NotifyHorizontalGeometryChanged(); });
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
            set => SetProperty(positionY, value, v
                =>
            { positionY = v; NotifyVerticalGeometryChanged(); });
        }
        #endregion

        #region Координата крайней точки по оси X
        /// <summary>
        /// Координата крайней точки по оси X
        /// </summary>
        public double Right
            => PositionX + Width;
        #endregion

        #region Координата крайней точки по оси Y
        /// <summary>
        /// Координата крайней точки по оси Y
        /// </summary>
        public double Bottom
            => PositionY + Height;
        #endregion

        #region Границы
        /// <summary>
        /// Границы
        /// </summary>
        public Rect Bounds
            => new(PositionX, PositionY, Width, Height);
        #endregion

        #region Уведомление об изменении геометрии
        /// <summary>
        /// Уведомление об изменении геометрии
        /// </summary>
        /// <param name="properties"></param>
        private void NotifyGeometryChanged(params string[] properties)
        {
            foreach (var property in properties)
                OnPropertyChanged(property);
        }
        #endregion

        #region Уведомление об изменении геометрии по горизонтали
        /// <summary>
        /// Уведомление об изменении геометрии по горизонтали
        /// </summary>
        protected void NotifyHorizontalGeometryChanged()
        {
            NotifyGeometryChanged(
                nameof(Right),
                nameof(Bounds));
        }
        #endregion

        #region Уведомление об изменении геометрии по вертикали
        /// <summary>
        /// Уведомление об изменении геометрии по вертикали
        /// </summary>
        protected void NotifyVerticalGeometryChanged()
        {
            NotifyGeometryChanged(
                nameof(Bottom),
                nameof(Bounds));
        }
        #endregion
    }
}