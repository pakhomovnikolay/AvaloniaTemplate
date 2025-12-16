using AvaloniaTemplate.ViewModels.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.ViewModels.Base
{
    public class ViewModelBase : ObservableObject, IViewModelBase
    {
        #region Title
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        #endregion

        #region WindowHeight
        private int windowHeight = 800;
        public int WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }
        #endregion

        #region WindowWidth
        private int windowWidth = 1600;
        public int WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        #endregion
    }
}
