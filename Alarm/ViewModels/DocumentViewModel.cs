using Alarm.ViewModels.Interface;
using Model.Interface;
using System;
using System.Linq;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public abstract class DocumentViewModel : ViewModelBase, IPageShow, IDocument
    {
        public abstract string Title { get; set; }
        public abstract string HostUri { get; set; }
        public abstract string PathUri { get; set; }
        public abstract string Summary { get; set; }
        public abstract DateTime Date { get; set; }
        public abstract string GUID { get; set; }
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

        private bool isSelected;

        protected DocumentViewModel(IViewModelBehavior behavior) : base(behavior) {}

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(isSelected));
            }
        }

        public string ShowingPageName => "ContentView";
        public Page ShowingPage { get; set; }

        public Page CreatePageShowing()
        {
            return new View.ContentView();
        }
    }
}