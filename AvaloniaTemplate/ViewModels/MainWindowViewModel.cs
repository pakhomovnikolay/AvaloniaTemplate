using AvaloniaTemplate.ViewModels.Base;

namespace AvaloniaTemplate.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Title = "AvaloniaTemplate";
            WindowHeight = 750;
            WindowWidth = 1000;
            App.ChangeAppStatus += ChangeAppStatus;

            AppVersion = App.AppVersion;
            AppStatus = App.AppStatus;
        }

        #region Текущая версия проекта
        private string appVersion;
        /// <summary>
        /// Текущая версия проекта
        /// </summary>
        public string AppVersion
        {
            get => appVersion;
            set => SetProperty(ref appVersion, value);
        }
        #endregion

        #region Текущее состояние проекта
        private string appStatus;
        /// <summary>
        /// Текущее состояние проекта
        /// </summary>
        public string AppStatus
        {
            get => appStatus;
            set => SetProperty(ref appStatus, value);
        }
        #endregion

        #region Событие изменения состояние приложения
        /// <summary>
        /// Событие изменения состояние приложения
        /// </summary>
        /// <param name="settings"></param>
        private void ChangeAppStatus(string status) => AppStatus = status;
        #endregion
    }
}
