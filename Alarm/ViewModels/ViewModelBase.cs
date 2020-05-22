using Alarm.ViewModels.Interface;
using System.ComponentModel;

namespace Alarm.ViewModels
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
