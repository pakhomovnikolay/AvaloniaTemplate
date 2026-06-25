using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.ViewModels.Base;

namespace AvaloniaTemplate.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Сервис обработки состояний
        /// <summary>
        /// Сервис обработки состояний
        /// </summary>
        public IGlobalStateService GlobalStateService { get; } = App.GetService<IGlobalStateService>(); 
        #endregion

        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel()
            => InitializeComponent();
        #endregion

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

        #region Инициализация компонентов
        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        private void InitializeComponent()
        {
            Title = "Мастер конфигурации проекта";
            WindowHeight = 750;
            WindowWidth = 1600;
            AppVersion = App.AppVersion;
            AppStatus = App.AppStatus;
            App.ChangeAppStatus += ChangeAppStatus;
        }
        #endregion
    }
}
