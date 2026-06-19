using AvaloniaTemplate.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTemplate.Services.Registrations
{
    public static class RegistrationService
    {
        public static void AddServices(this IServiceCollection services) => services
            .AddSingleton<IClipboardService, ClipboardService>()
            .AddSingleton<IEncryptorService, EncryptorService>()
            .AddSingleton<ILogService, LogService>()
            .AddSingleton<IOperationService, OperationService>()
            .AddSingleton<IPropertyAccessorFactory, PropertyAccessorFactory>()
            .AddSingleton<IUserDialogService, UserDialogService>()
            .AddSingleton<IGlobalStateService, GlobalStateService>()

            //.AddSingleton<IGlobalStateService, GlobalStateService>()
            ;
    }
}
