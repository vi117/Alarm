using System.Windows.Controls;

namespace Alarm.ViewModels.Interface
{
    public interface ICategoryViewModel : IPageShow, IViewModelBase
    {
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        CollectionViewModel<ISiteViewModel> SiteModels { get; set; }
        string Title { get; set; }
    }
}