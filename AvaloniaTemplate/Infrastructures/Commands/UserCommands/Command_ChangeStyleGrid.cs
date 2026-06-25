using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    /// <summary>
    /// Команда - сменить стиль сетки
    /// </summary>
    public class Command_ChangeStyleGrid : Command
    {
        protected override bool CanExecute(object p)
            => p is not null && p is CurrentBorderStyleType;

        protected override void Execute(object p)
        {
            if (p is null || p is not CurrentBorderStyleType style)
                return;

            var stateService = App.GetService<IGlobalStateService>();
            stateService.BorderStyleType = style;

            Debug.WriteLine($"Текущий стиль сетки: {style}");


            App.GetService<IUserDialogService>()?
                .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
