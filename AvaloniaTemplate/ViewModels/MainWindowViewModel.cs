using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

        #region Путь к источнику файла
        private string filePath;
        /// <summary>
        /// Путь к источнику файла
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }
        #endregion

        #region Путь к источнику файла
        private string targetFilePath;
        /// <summary>
        /// Путь к источнику файла
        /// </summary>
        public string TargetFilePath
        {
            get => targetFilePath;
            set => SetProperty(ref targetFilePath, value);
        }
        #endregion

        #region Прогресс шифрования
        private double proccess = 0.3;
        /// <summary>
        /// Прогресс шифрования
        /// </summary>
        public double Proccess
        {
            get => proccess;
            set => SetProperty(ref proccess, value);
        }
        #endregion

        #region Команда - выбрать файл
        /// <summary>
        /// Команда - выбрать файл
        /// </summary>
        public ICommand Command_SelectedFileSource
            => new RelayCommandAsync(ExecuteCommand_SelectedFileSource);

        private async Task ExecuteCommand_SelectedFileSource()
        {
            Proccess = 0;
            FilePath = await App.GetService<IUserDialogService>()?
                .SelectFileAsync("Выбрать файл", defaultPath: Path.GetDirectoryName(FilePath));

            //if (string.IsNullOrWhiteSpace(path))
            //    return;

            //Proccess = 0;
            //FilePath = path;
            //var fileName = Path.GetFileNameWithoutExtension(path);
            //TargetFilePath = Path.Combine(Path.GetDirectoryName(FilePath), fileName + ".fileencrypted");
        }
        #endregion

        #region Команда - выбрать файл
        /// <summary>
        /// Команда - выбрать файл
        /// </summary>
        public ICommand Command_SelectedFileTarget
            => new RelayCommandAsync(ExecuteCommand_SelectedFileTarget);

        private async Task ExecuteCommand_SelectedFileTarget()
        {
            Proccess = 0;
            TargetFilePath = await App.GetService<IUserDialogService>()?
                .SelectFileAsync("Выбрать файл", defaultPath: Path.GetDirectoryName(FilePath));

            //if (string.IsNullOrWhiteSpace(path))
            //    return;

            //Proccess = 0;
            //FilePath = path;
            //var fileName = Path.GetFileNameWithoutExtension(path);
            //TargetFilePath = Path.Combine(Path.GetDirectoryName(FilePath), fileName + ".fileencrypted");
        }
        #endregion


        #region Команда - зашифровать
        /// <summary>
        /// Команда - зашифровать
        /// </summary>
        public ICommand Command_Encryption
            => new RelayCommandAsync(ExecuteCommand_Encryption, CanExecuteCommand_Encryption);

        private bool CanExecuteCommand_Encryption(object p)
            => File.Exists(FilePath);

        private async Task ExecuteCommand_Encryption(object p)
        {
            var fileName = Path.GetFileNameWithoutExtension(FilePath);
            var targetFilePath = Path.Combine(Path.GetDirectoryName(FilePath), fileName + ".encrypted");

            var result = await App.GetService<IEncryptorService>()?
                .EncryptStreamAsync(FilePath, targetFilePath, progress: new Progress<double>(p => Proccess = p));

            if (result.Success)
                Debug.WriteLine(result.Success);
            else
                Debug.WriteLine(result.Error);





        }
        #endregion

        #region Команда - расшифровать
        /// <summary>
        /// Команда - расшифровать
        /// </summary>
        public ICommand Command_Decryption
            => new RelayCommandAsync(ExecuteCommand_Decryption, CanExecuteCommand_Decryption);

        private bool CanExecuteCommand_Decryption(object p)
            => File.Exists(TargetFilePath);

        private async Task ExecuteCommand_Decryption(object p)
        {
            var fileName = Path.GetFileNameWithoutExtension(TargetFilePath);
            var sourceFilePath = Path.Combine(Path.GetDirectoryName(TargetFilePath), fileName + ".txt");

            var result = await App.GetService<IEncryptorService>()?
                .DecryptStreamAsync(TargetFilePath, sourceFilePath, progress: new Progress<double>(p => Proccess = p));

            if (result.Success)
                Debug.WriteLine(result.Success);
            else
                Debug.WriteLine(result.Error);
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
