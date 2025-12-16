using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public class CommandProvider(IServiceProvider service, Dictionary<string, Type> commandsMap) : ICommandProvider
    {
        private readonly IServiceProvider provider = service;
        private readonly Dictionary<string, Type> commands = commandsMap;

        #region Получить команду по ключу
        /// <summary>
        /// Получить команду по ключу
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICommand GetCommand(string name)
        {
            if (commands.TryGetValue(name, out var type))
                return (ICommand)provider.GetRequiredService(type);

            throw new KeyNotFoundException($"Команда '{name}' не зарегистрирована.");
        }
        #endregion
    }
}
