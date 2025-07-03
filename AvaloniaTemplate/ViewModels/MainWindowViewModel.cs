using AvaloniaTemplate.ViewModels.Base;

namespace AvaloniaTemplate.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            VersionApp = App.Version;
        }

        #region Текущая версия проекта
        private string versionApp;
        /// <summary>
        /// Текущая версия проекта
        /// </summary>
        public string VersionApp
        {
            get => versionApp;
            set => SetProperty(ref versionApp, value);
        }
        #endregion
    }
}
