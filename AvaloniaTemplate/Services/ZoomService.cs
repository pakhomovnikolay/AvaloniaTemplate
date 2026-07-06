using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Services
{
    public class ZoomService : ObservableObject, IZoomService
    {
        #region Масштаб
        private double scale = 1;
        /// <summary>
        /// Масштаб
        /// </summary>
        public double Scale
        {
            get => scale;
            private set => SetProperty(ref scale, value);
        }
        #endregion

        #region Минимальная значение шага изменения мастаба
        private double smallChangeScale = 0.5;
        /// <summary>
        /// Минимальная значение шага изменения мастаба
        /// </summary>
        public double SmallChangeScale
        {
            get => smallChangeScale;
            set => SetProperty(ref smallChangeScale, value);
        }
        #endregion

        #region Установить масштаб
        /// <summary>
        /// Установить масштаб
        /// </summary>
        /// <param name="scale"></param>
        public void SetScale(double scale)
            => Scale = scale;
        #endregion

        #region Пересчитать масштаб
        /// <summary>
        /// Пересчитать масштаб
        /// </summary>
        /// <param name="scale"></param>
        public void RecalculateScale(double scale)
            => Scale += scale * (smallChangeScale / 10);
        #endregion
    }
}
