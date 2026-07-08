using AvaloniaTemplate.ViewModels.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.ViewModels.Base
{
    public class ViewModelBase : ObservableObject, IViewModelBase
    {
        #region Заголовок окна
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        #endregion

        #region Высота окна
        private int windowHeight = 800;
        public virtual int WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }
        #endregion

        #region Ширина окна
        private int windowWidth = 1600;
        public virtual int WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        #endregion
    }
}
