using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    /// <summary>
    /// Команда - вставить данные из буфера обмена
    /// </summary>
    public class Command_Paste : Command
    {
        protected override bool CanExecute(object p)
            => p is not null && p is bool state && state;

        protected override void Execute(object p)
        {
            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
