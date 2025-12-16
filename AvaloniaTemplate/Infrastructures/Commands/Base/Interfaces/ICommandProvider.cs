using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces
{
    public interface ICommandProvider
    {
        #region Получить команду по ключу
        /// <summary>
        /// Получить команду по ключу
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICommand GetCommand(string name);
        #endregion
    }
}
