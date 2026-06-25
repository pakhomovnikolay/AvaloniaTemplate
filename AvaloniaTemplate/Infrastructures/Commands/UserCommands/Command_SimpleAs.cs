using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    /// <summary>
    /// Команда - применить выбранный стиль
    /// </summary>
    public class Command_SimpleAs : Command
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
