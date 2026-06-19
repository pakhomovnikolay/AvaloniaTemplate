using AvaloniaTemplate.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTemplate.Services.Registrations
{
    public static class RegistrationViewModelsService
    {
        public static void AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()

            //.AddSingleton<MainWindowViewModel>()
            ;
    }
}
