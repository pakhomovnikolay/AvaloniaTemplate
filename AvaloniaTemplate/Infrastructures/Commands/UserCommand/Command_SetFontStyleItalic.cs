using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    /// <summary>
    /// Команда - Установить курсивный стиль текста
    /// </summary>
    public class Command_SetFontStyleItalic : Command
    {
        protected override bool CanExecute(object p)
            => true;

        protected override void Execute(object p)
        {
            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
