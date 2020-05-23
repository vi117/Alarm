using ViewModel.Interface;
using System.ComponentModel;

namespace ViewModel
{
    public abstract class ViewModelBase : IViewModelBase
    {
        public IViewModelBase Parent { get; set; }

        public ViewModelBase() { Parent = null; }
        public ViewModelBase(IViewModelBase parent)
        {
            Parent = parent;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
