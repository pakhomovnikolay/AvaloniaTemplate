using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommand
{
    /// <summary>
    /// Команда - обяединить ячейки
    /// </summary>
    public class Command_MergeCells : Command
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
