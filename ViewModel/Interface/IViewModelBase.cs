using System.ComponentModel;

namespace ViewModel.Interface
{
    /// <summary>
    /// Base interface of All View Model
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
        IViewModelBase Parent { get; set; }
    }
}