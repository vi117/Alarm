using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public class MockFetcherViewModel : FetcherViewModel
    {
        private string title;
        private CollectionViewModel<DocumentViewModel> documents;

        public MockFetcherViewModel()
        {
            documents = new CollectionViewModel<DocumentViewModel>();
            IsSelected = false;
            IsExpanded = false;
        }
        public MockFetcherViewModel(ViewModel viewModel, string title)
        {
            Root = viewModel;
            this.title = title;
            documents = new CollectionViewModel<DocumentViewModel>(Root);
            IsSelected = false;
            IsExpanded = false;
        }
        public override string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public override CollectionViewModel<DocumentViewModel> Documents
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
        public void RemoveDocument(MockDocumentViewModel document)
        {
            documents.Remove(document);
            OnPropertyChanged(nameof(Documents));
        }
        public void Add(MockDocumentViewModel document)
        {
            documents.Add(document);
            OnPropertyChanged(nameof(Documents));
        }
    }
}
