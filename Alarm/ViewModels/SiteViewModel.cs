using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels
{
    public class SiteViewModel : ViewModelBase, IAlertPage
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
        public SiteViewModel(string title) : this()
        {
            this.title = title;
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

        public string ValidPageName => "ContentListView";

        public void RemoveFirstDocument()
        {
            documents.Remove(documents.First());
            OnPropertyChanged(nameof(Documents));
        }
        public void RemoveDocument(DocumentViewModel document)
        {
            OnPropertyChanged(nameof(Documents));
        }
        public void Add(DocumentViewModel document)
        {
            documents.Add(document);
            OnPropertyChanged(nameof(Documents));
        }
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
