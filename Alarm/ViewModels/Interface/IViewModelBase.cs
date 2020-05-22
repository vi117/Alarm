using System.ComponentModel;

namespace Alarm.ViewModels.Interface
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        IViewModelBase Parent { get; set; }
    }
}