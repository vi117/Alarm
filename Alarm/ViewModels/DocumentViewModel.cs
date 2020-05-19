using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Windows.Controls;
using Model.Interface;
using Alarm.ViewModels.Interface;

namespace Alarm.ViewModels
{
    public class DocumentViewModel : ViewModelBase, IDocumentViewModel
    {
        public string Title { get; set; }

        public string HostUri { get; set; }
        public string PathUri { get; set; }
        public string Uri
        {
            get => HostUri + "/" + PathUri;
            set
            {
                var uri = new Uri(value);
                HostUri = uri.Host;
                PathUri = uri.PathAndQuery;
            }
        }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string GUID { get; set; }
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(isSelected));
            }
        }

        public DocumentViewModel() : base()
        { }
        public DocumentViewModel(IDocument document) : base()
        {
            HostUri = document.HostUri;
            PathUri = document.PathUri;
            Date = document.Date;
            GUID = document.GUID;
            Summary = document.Summary;
            Title = document.Title;
        }
        public string ShowingPageName => "ContentView";
        public Page ShowingPage { get; set; }
        public Page CreatePageShowing()
        {
            return new View.ContentView();
        }
    }
}
