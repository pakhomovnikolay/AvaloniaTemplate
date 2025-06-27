using AvaloniaTemplate.ViewModels;
using AvaloniaTemplate.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTemplate.Services.Registrations
{
    public static class RegistrationWindowsService
    {
        public static void AddWindows(this IServiceCollection services) => services
            .AddSingleton(s =>
            {
                var model = s.GetRequiredService<MainWindowViewModel>();
                var window = new MainWindow { DataContext = model }; return window;
            })
            ;
    }
}
