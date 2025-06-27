using AvaloniaTemplate.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTemplate.Services.Registrations
{
    public static class RegistrationService
    {
        public static void AddServices(this IServiceCollection services) => services
            .AddSingleton<IUserDialogService, UserDialogService>()
            .AddSingleton<IEncryptorService, EncryptorService>()
            ;
    }
}
