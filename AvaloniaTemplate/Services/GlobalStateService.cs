using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.Services
{
    public class GlobalStateService : ObservableObject, IGlobalStateService
    {
        #region Индекс выбранного из списка размера шрифта
        private int selectedIndexFontSize;
        public int SelectedIndexFontSize
        {
            get => selectedIndexFontSize;
            set => SetProperty(ref selectedIndexFontSize, value);
        }
        #endregion
    }
}
