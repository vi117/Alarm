using System.ComponentModel;

namespace Alarm.ViewModels.Interface
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        IViewModelBehavior Root { get; set; }
    }
}