using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaTemplate.Services.Interfaces;
using AvaloniaTemplate.ViewModels;
using AvaloniaTemplate.Views;
using AvaloniaTemplate.Views.UserControls;
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
            .AddSingleton(s =>
            {
                var model = s.GetRequiredService<PresenterTableViewModel>();
                var window = new PresenterTable { DataContext = model };
                window.AddHandler(
                    InputElement.PointerWheelChangedEvent,
                    s.GetRequiredService<IInputManagerService>().PointerWheelHandler,
                    RoutingStrategies.Tunnel,
                    true
                    );
                return window;
            })


            

            //.AddSingleton(s =>
            //{
            //    var model = s.GetRequiredService<MainWindowViewModel>();
            //    var window = new MainWindow { DataContext = model }; return window;
            //})
            ;
    }
}
