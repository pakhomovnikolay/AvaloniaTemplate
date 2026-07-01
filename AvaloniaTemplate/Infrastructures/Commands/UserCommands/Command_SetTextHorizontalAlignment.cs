using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    public class Command_SetTextHorizontalAlignment : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not HorizontalAlignment alignment)
                return;

            //Debug.WriteLine($"HorizontalAlignment: {alignment}");
            //if (App.GetService<IUIConnectorService>() is { } service)
            //    service.HorizontalTextAlignment = alignment;

            //App.GetService<IUserDialogService>()?
            //    .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
