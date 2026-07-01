using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Services.Interfaces;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    public class Command_SetForegroundStyle : Command
    {
        protected override bool CanExecute(object p)
            => p is not null;

        protected override void Execute(object p)
        {
            if (p is null || p is not IBrush color)
                return;

            //var stateService = App.GetService<IUIConnectorService>();
            //stateService.CurrentForeground = color;
            //if (!stateService.ForegroundColors.Contains(Color.Parse(color.ToString())))
            //{
            //    stateService.ForegroundColors.Insert(0, Color.Parse(color.ToString()));
            //    if (stateService.ForegroundColors.Count > 10)
            //        stateService.ForegroundColors.RemoveAt(stateService.BackgroundColors.Count - 1);
            //}

            //Debug.WriteLine($"Текущий цвет заливки: {color}");

            //App.GetService<IUserDialogService>()?
            //    .SendMessageAsync("Команда", "Реализуйте команду: " + GetType().Name, App.Desktop.MainWindow);
        }
    }
}
