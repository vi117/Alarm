using Alarm.ViewModels.Interface;
using System.ComponentModel;

namespace Alarm.ViewModels
{
    public abstract class ViewModelBase : IViewModelBase
    {
        private IViewModelBehavior root;
        //for designer mode
        public ViewModelBase() { root = null; }
        public ViewModelBase(IViewModelBehavior behavior)
        {
            root = behavior;
        }
        public IViewModelBehavior Root
        {
            get => root;
            set => root = value;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
