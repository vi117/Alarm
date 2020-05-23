using System.ComponentModel;

namespace ViewModel.Interface
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        IViewModelBase Parent { get; set; }
    }
}