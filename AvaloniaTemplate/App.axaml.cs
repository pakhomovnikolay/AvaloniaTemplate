using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.Services.Registrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AvaloniaTemplate
{
    public partial class App : Application
    {
        private static IClassicDesktopStyleApplicationLifetime desktop;
        private static IServiceProvider services;
        private static string appStatus;
        private static bool requestConfirmCloseBeforeClosing = Helper.GetResource<bool>("RequestConfirmCloseBeforeClosing");
        private static bool appDataChanged = false;

        private static readonly string emblemCompany = "CTEKSLogo.png";
        private static readonly string nameCompany = "ООО «КиберТЭКСистем»";
        private static readonly string webAddressCompany = "cteks.ru";
        private static readonly string emailSupportCompany = "info@cteks.ru";
        private static readonly string signatureCompany = "© 2026 ООО «КТС». Все права защищены";

        #region Событие изменения статуса приложения
        /// <summary>
        /// Событие изменения статуса приложения
        /// </summary>
        public static event ChangedAppStatus ChangeAppStatus;
        public delegate void ChangedAppStatus(string status);
        #endregion

        #region Получить провайдера сервисов
        /// <summary>
        /// Получить провайдера сервисов
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider Services => services;
        #endregion

        #region Получить текущую версию проекта
        /// <summary>
        ///  Получить текущую версию проекта
        /// </summary>
        public static string AppVersion => $"Версия: v.{Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ??
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ??
            "0.0.0.0"}";
        #endregion

        #region Получить имя приложения
        /// <summary>
        /// Получить имя приложения
        /// </summary>
        public static string AppName => Assembly.GetEntryAssembly().GetName().Name;
        #endregion

        #region Путь к директории хранения настроек
        /// <summary>
        /// Путь к директории хранения настроек
        /// </summary>
        public static string FolderPathSettings => Path.Combine(Directory.GetCurrentDirectory(), $"{AppName}Settings");
        #endregion

        #region Путь к директории хранения логов
        /// <summary>
        /// Путь к директории хранения логов
        /// </summary>
        public static string FolderPathLogs => Path.Combine(Directory.GetCurrentDirectory(), $"{AppName}Logs");
        #endregion

        #region Путь к директории приложения
        /// <summary>
        /// Путь к директории приложения
        /// </summary>
        public static string FolderPath => Path.Combine(Directory.GetCurrentDirectory());
        #endregion

        #region Текущий статус приложения
        /// <summary>
        /// Текущий статус приложения
        /// </summary>
        public static string AppStatus => appStatus;
        #endregion

        #region Установить статус приложения
        /// <summary>
        /// Установить статус приложения
        /// </summary>
        /// <param name="content"></param>
        public static void SetAppStatus(string content = "Готов")
        {
            var status = "Текущее состояние: ";
            status += (string.IsNullOrEmpty(content) ? "Готов" : content);

            if (appStatus != status)
            {
                appStatus = status;
                ChangeAppStatus?.Invoke(status);
            }
        }
        #endregion

        #region Получить данные приложения
        /// <summary>
        /// Получить данные приложения
        /// </summary>
        public static IClassicDesktopStyleApplicationLifetime Desktop => desktop;
        #endregion

        #region Получить TopLevel
        /// <summary>
        /// Получить TopLevel
        /// </summary>
        /// <returns></returns>
        public static TopLevel GetTopLevel()
            => TopLevel.GetTopLevel(desktop.MainWindow);
        #endregion

        #region Получить текущее состояние необходимости запроса подтверждения закрытия приложения
        /// <summary>
        /// Получить текущее состояние необходимости запроса подтверждения закрытия приложения
        /// </summary>
        /// <returns></returns>
        public static bool GetStateRequestConfirmCloseBeforeClosing()
            => requestConfirmCloseBeforeClosing;
        #endregion

        #region Изменить необходимость подтверждения закрытия приложения
        /// <summary>
        /// Изменить необходимость подтверждения закрытия приложения
        /// </summary>
        /// <param name="confirm"></param>
        public static void ChangeConfirmCloseBeforeClosing(bool confirm)
            => requestConfirmCloseBeforeClosing = confirm;
        #endregion

        #region Получить текущее состояние изменения данных в проекте
        /// <summary>
        /// Получить текущее состояние изменения данных в проекте
        /// </summary>
        /// <returns></returns>
        public static bool GetStatusAppDataChanged()
            => appDataChanged;
        #endregion

        #region Установить состояние изменения данных в проекте
        /// <summary>
        /// Установить состояние изменения данных в проекте
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeStatusAppDataChanged(bool change)
        {
            appDataChanged = change;
        }
        #endregion

        #region Получить указанный сервис
        /// <summary>
        /// Получить указанный сервис
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
            => Services.GetRequiredService<T>();
        #endregion

        #region Эмблема компании
        /// <summary>
        /// Эмблема компании
        /// </summary>
        public static string EmblemCompany => emblemCompany;
        #endregion

        #region Наименование компании
        /// <summary>
        /// Наименование компании
        /// </summary>
        public static string NameCompany => nameCompany;
        #endregion

        #region Веб адрес компании
        /// <summary>
        /// Веб адрес компании
        /// </summary>
        public static string WebAddressCompany => webAddressCompany;
        #endregion

        #region Адрес тех. поддержки компании
        /// <summary>
        /// Адрес тех. поддержки компании
        /// </summary>
        public static string EmailSupportCompany => emailSupportCompany;
        #endregion

        #region Подпись компании
        /// <summary>
        /// Подпись компании
        /// </summary>
        public static string SignatureCompany => signatureCompany;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Initialize()
            => AvaloniaXamlLoader.Load(this);

        /// <summary>
        /// Инициализация компонентов при старте приложения 
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app)
            {
                SetAppStatus();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var Services = new ServiceCollection();
                Services.AddCommands();
                Services.AddServices();
                Services.AddViewModels();
                Services.AddWindows();
                services = Services.BuildServiceProvider();

                desktop = app;
                desktop.MainWindow = services.GetRequiredService<IUserDialogService>().GetMainWindow();
                DisableAvaloniaDataAnnotationValidation();
            }
            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Действия выполняемые после инициализации старта приложения
        /// </summary>
        private static void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
                BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}