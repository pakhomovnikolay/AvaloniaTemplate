using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    /// <summary>
    /// Команда - Установить подчеркнутый стиль текста
    /// </summary>
    public class Command_SetTextStyleUnderline : Command
    {
        protected override bool CanExecute(object p)
            => true;

        protected override void Execute(object p)
        {
            //var stateService = App.GetService<IGlobalStateService>();
            //stateService.BorderStyleType = style;

            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
