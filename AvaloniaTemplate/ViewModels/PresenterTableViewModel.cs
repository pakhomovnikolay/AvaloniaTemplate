using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.ViewModels
{
    public class PresenterTableViewModel : ObservableObject
    {
        #region Сервис обработки состояний
        /// <summary>
        /// Сервис обработки состояний
        /// </summary>
        public IUIConnectorService ConnectorService { get; } = App.GetService<IUIConnectorService>();
        #endregion

        #region Сервис управления моделями таблиц
        /// <summary>
        /// Сервис управления моделями таблиц
        /// </summary>
        public ITableGenerateFactory TablesFactory { get; } = App.GetService<ITableGenerateFactory>();
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

        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        public PresenterTableViewModel()
            => InitializeComponent();
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

        #region Высота окна
        private int windowHeight = 800;
        public virtual int WindowHeight
        {
            get => windowHeight;
            set
            {
                if (SetProperty(ref windowHeight, value))
                {
                    ConnectorService.WindowHeight = windowHeight;
                    TablesFactory.UpdateViewport();
                }

            }
        }
        #endregion

        #region Ширина окна
        private int windowWidth = 1600;
        public virtual int WindowWidth
        {
            get => windowWidth;
            set
            {
                if (SetProperty(ref windowWidth, value))
                {
                    ConnectorService.WindowWidth = windowWidth;
                    TablesFactory.UpdateViewport();
                }
            }
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
            AppStatus = App.AppStatus;
            App.ChangeAppStatus += ChangeAppStatus;
        }
        #endregion
    }
}
