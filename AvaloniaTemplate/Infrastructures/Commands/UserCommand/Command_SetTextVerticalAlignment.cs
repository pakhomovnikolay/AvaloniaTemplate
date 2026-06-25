using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    public class Command_SetTextVerticalAlignment : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not VerticalAlignment alignment)
                return;

            Debug.WriteLine($"VerticalAlignment: {alignment}");
            if (App.GetService<IGlobalStateService>() is { } service)
                service.VerticalTextAlignment = alignment;

            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
