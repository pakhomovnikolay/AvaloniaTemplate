using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.ViewModels.Base;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Сервис обработки состояний
        /// <summary>
        /// Сервис обработки состояний
        /// </summary>
        public IUIConnectorService ConnectorService { get; } = App.GetService<IUIConnectorService>(); 
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

            ConnectorService.CollectionHorizontalTabStrip = ["1", "2", "3", "4", "5", "6", "7", "81", "9", "10", "11"];
            ConnectorService.SelectedItemHorizontalTabStrip = ConnectorService.CollectionHorizontalTabStrip[0];
        }
        #endregion

        #region Текущая версия проекта
        private ObservableCollection<string> testItemList;
        /// <summary>
        /// Текущая версия проекта
        /// </summary>
        public ObservableCollection<string> TestItemList
        {
            get => testItemList;
            set => SetProperty(ref testItemList, value);
        }
        #endregion


        #region Текущая версия проекта
        private string testItemSelected;
        /// <summary>
        /// Текущая версия проекта
        /// </summary>
        public string TestItemSelected
        {
            get => testItemSelected;
            set => SetProperty(ref testItemSelected, value);
        }
        #endregion
    }
}
