using Avalonia.Markup.Xaml;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public class CommandMarkupExtension(string name) : MarkupExtension
    {
        public string Name { get; } = name;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provider = serviceProvider.GetRequiredService<ICommandProvider>();
            return provider.GetCommand(Name);
        }
    }
}
