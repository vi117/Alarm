using Model.Interface;

namespace Alarm.ViewModels.Interface
{
    public interface IDocumentViewModel: IPageShow, IDocument, IViewModelBase
    {
        bool IsSelected { get; set; }
        string Uri { get; set; }
    }
}