using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    /// <summary>
    /// Команда - предварительного просмотра выбранного размера шрифта
    /// </summary>
    public class Command_PreViewSelectedFontSize : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not double fontSize)
                return;

            Debug.WriteLine($"Текущий размер шрифта: {fontSize}");


            //App.GetService<IUserDialogService>()?
            //    .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
