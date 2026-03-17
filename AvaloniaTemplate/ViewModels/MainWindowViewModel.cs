using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaTemplate.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel()
        {
            Title = "AvaloniaTemplate";
            WindowHeight = 750;
            WindowWidth = 1000;
            App.ChangeAppStatus += ChangeAppStatus;

            AppVersion = App.AppVersion;
            AppStatus = App.AppStatus;
        }
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





        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region Текст
        private string text;
        /// <summary>
        /// Текст
        /// </summary>
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
        #endregion

        #region isEnabled
        private bool isEnabled = true;
        /// <summary>
        /// Текст
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
        #endregion


        #region Команда - открыть
        /// <summary>
        /// Команда - открыть
        /// </summary>
        public ICommand Command_Enabled
            => new RelayCommand(ExecuteCommand_Enabled);

        private void ExecuteCommand_Enabled()
        {
            IsEnabled = !IsEnabled;
        }
        #endregion


        #region Текст
        private ObservableCollection<string> sourceItems = ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6", "Item 7", "Item 8", "Item 9"];
        /// <summary>
        /// Текст
        /// </summary>
        public ObservableCollection<string> SourceItems
        {
            get => sourceItems;
            set => SetProperty(ref sourceItems, value);
        }
        #endregion

        #region Текст
        private ObservableCollection<string> targetItems = [];
        /// <summary>
        /// Текст
        /// </summary>
        public ObservableCollection<string> TargetItems
        {
            get => targetItems;
            set => SetProperty(ref targetItems, value);
        }
        #endregion
    }
}
