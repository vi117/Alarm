using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public class SiteViewModel : ViewModelBase, IPageShow
    {
        private bool isSelected;
        private bool isExpanded;
        private string title;
        private CollectionViewModel<DocumentViewModel> documents;

        public SiteViewModel()
        {
            documents = new CollectionViewModel<DocumentViewModel>();
            isSelected = false;
            isExpanded = false;
        }
        public SiteViewModel(ViewModel viewModel,string title)
        {
            Root = viewModel;
            this.title = title;
            documents = new CollectionViewModel<DocumentViewModel>(Root);
            isSelected = false;
            isExpanded = false;
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public CollectionViewModel<DocumentViewModel> Documents
        {
            get => documents;
            set
            {
                documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }


        public void RemoveFirstDocument()
        {
            documents.Remove(documents.First());
            OnPropertyChanged(nameof(Documents));
        }
        public void RemoveDocument(DocumentViewModel document)
        {
            documents.Remove(document);
            OnPropertyChanged(nameof(Documents));
        }
        public void Add(DocumentViewModel document)
        {
            documents.Add(document);
            OnPropertyChanged(nameof(Documents));
        }

        public string ShowingPageName => "ContentListView";
        public Page CreatePageShowing()
        {
            return new View.ContentListView();
        }
        public Page ShowingPage { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    OnPropertyChanged(nameof(isExpanded));
                }
            }
        }
    }
}
