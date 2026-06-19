using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using AvaloniaTemplate.Infrastructures.Commands.UserCommand;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Services.Registrations
{
    public static class RegistrationCommands
    {
        private static readonly Dictionary<string, Type> commands = [];
        private static void RegisterCommand<T>(string name) where T : ICommand
            => commands[name] = typeof(T);

        public static void AddCommands(this IServiceCollection services)
        {
            // 1. Создаем команды
            RegisterCommand<Command_Paste>("Command_Paste");
            RegisterCommand<Command_Cut>("Command_Cut");
            RegisterCommand<Command_Copy>("Command_Copy");
            RegisterCommand<Command_SimpleAs>("Command_SimpleAs");
            RegisterCommand<Command_PreViewSelectedFont>("Command_PreViewSelectedFont");

            //RegisterCommand<Command_PreViewSelectedFont>("Command_PreViewSelectedFont");

            // 2. Регистрируем все типы команд в DI
            foreach (var kvp in commands)
            {
                services.AddSingleton(kvp.Value);
                services.AddKeyedSingleton(kvp.Value, (sp, _) => (ICommand)sp.GetRequiredService(kvp.Value));
            }

            // 3. Регистрируем провайдер
            services.AddSingleton<ICommandProvider>(sp => new CommandProvider(sp, commands));
        }
    }
}
