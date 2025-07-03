using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.Services.Registrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace AvaloniaTemplate
{
    public partial class App : Application
    {
        private static IClassicDesktopStyleApplicationLifetime desktop;
        private static IServiceProvider services;

        #region Получить провайдера сервисов
        /// <summary>
        /// Получить провайдера сервисов
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider Services => services;
        #endregion

        #region Получить текущую версию проекта
        /// <summary>
        /// Получить текущую версию проекта
        /// </summary>
        /// <returns></returns>
        public static string Version => $"Версия: v.{Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ??
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ??
            "0.0.0.0"}";
        #endregion

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var Services = new ServiceCollection();
                Services.AddServices();
                Services.AddViewModels();
                Services.AddWindows();
                services = Services.BuildServiceProvider();
                services.GetRequiredService<IUserDialogService>().OpenMainWindow();

                desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                desktop.MainWindow = services.GetRequiredService<IUserDialogService>().GetMainWindow();

                DisableAvaloniaDataAnnotationValidation();
            }
            base.OnFrameworkInitializationCompleted();
        }

        private static void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}