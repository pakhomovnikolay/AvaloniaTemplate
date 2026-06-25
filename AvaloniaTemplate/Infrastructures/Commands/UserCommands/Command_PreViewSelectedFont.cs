using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
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
            if (p is null || p is not FontFamily font)
                return;

            Debug.WriteLine($"Текущий шрифт: {font.Name}");


            //App.GetService<IUserDialogService>()?
            //    .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
