using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels.Interface
{
    public interface ISiteViewModel : IPageShow, IViewModelBase
    {
        CollectionViewModel<IDocumentViewModel> Documents { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        string Title { get; set; }
    }
}
