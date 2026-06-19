using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    /// <summary>
    /// Команда - предварительного просмотра выбранного шрифта
    /// </summary>
    public class Command_PreViewSelectedFont : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not string font)
                return;

            Debug.WriteLine($"Текущий шрифт: {font}");


            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
