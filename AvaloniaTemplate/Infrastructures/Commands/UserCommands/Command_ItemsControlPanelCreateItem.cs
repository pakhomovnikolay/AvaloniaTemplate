using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.ViewModels;

namespace AvaloniaTemplate.Infrastructures.Commands.UserCommands
{
    /// <summary>
    /// Команда - создать элемент в коллекции
    /// </summary>
    public class Command_ItemsControlPanelCreateItem : Command
    {
        protected override bool CanExecute(object p)
            => true;

        protected override void Execute(object p)
        {
            var viewModel = App.GetService<MainWindowViewModel>();
            viewModel.TestItemList.Add($"{viewModel.TestItemList.Count + 1}");

            viewModel.TestItemSelected = Helper.GetSelectedElement<string>(viewModel.TestItemList.Count - 1, viewModel.TestItemList);
        }
    }
}
