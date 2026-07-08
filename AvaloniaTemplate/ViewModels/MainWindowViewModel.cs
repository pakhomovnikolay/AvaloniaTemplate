using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Services;
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

        #region Сервис управления панелью со вкладками
        /// <summary>
        /// Сервис управления панелью со вкладками
        /// </summary>
        public IHorizontalTabStripService<ModelTable> TabStripService { get; } = App.GetService<IHorizontalTabStripService<ModelTable>>();
        #endregion

        #region Сервис управления панелью прокрутки
        /// <summary>
        /// Сервис управления панелью прокрутки
        /// </summary>
        public IScrollBarService ScrollBarService { get; } = App.GetService<IScrollBarService>();
        #endregion

        #region Сервис управления масштабом
        /// <summary>
        /// Сервис управления масштабом
        /// </summary>
        public IZoomService ZoomService { get; } = App.GetService<IZoomService>();
        #endregion

        #region Сервис управления моделями таблиц
        /// <summary>
        /// Сервис управления тмоделями таблиц
        /// </summary>
        public ITableGenerateFactory TablesFactory { get; } = App.GetService<ITableGenerateFactory>();
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

            //TabStripService.ItemsSource = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"];
            //TabStripService.SelectedItem = TabStripService.ItemsSource[0];
            //TabStripService.CreateItem += () => { return $"{TabStripService.ItemsSource.Count + 1}"; };
        }
        #endregion
    }
}
