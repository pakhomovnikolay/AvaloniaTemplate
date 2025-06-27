using AvaloniaTemplate.ViewModels.Base.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaTemplate.ViewModels.Base
{
    public class ViewModelBase : ObservableObject, IViewModelBase
    {
        #region Title
        private string pTitle;
        public string Title
        {
            get => pTitle;
            set => SetProperty(ref pTitle, value);
        }
        #endregion

        #region WindowHeight
        private int pWindowHeight = 800;
        public int WindowHeight
        {
            get => pWindowHeight;
            set => SetProperty(ref pWindowHeight, value);
        }
        #endregion

        #region WindowWidth
        private int pWindowWidth = 1600;
        public int WindowWidth
        {
            get => pWindowWidth;
            set => SetProperty(ref pWindowWidth, value);
        }
        #endregion
    }
}
