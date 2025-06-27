namespace AvaloniaTemplate.ViewModels.Base.Interfaces
{
    public interface IViewModelBase
    {
        #region Заголовок окна
        /// <summary>
        /// Заголовок окна
        /// </summary>
        string Title { get; set; }
        #endregion

        #region Высота окна
        /// <summary>
        /// Высота окна
        /// </summary>
        int WindowHeight { get; set; }
        #endregion

        #region Ширина окна
        /// <summary>
        /// Ширина окна
        /// </summary>
        int WindowWidth { get; set; }
        #endregion
    }
}
