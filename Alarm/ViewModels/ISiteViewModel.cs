using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public interface ISiteViewModel : IPageShow
    {
        CollectionViewModel<DocumentViewModel> Documents { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        string Title { get; set; }
    }
}