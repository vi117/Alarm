using ViewModel.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Interface;

namespace ViewModel
{
    public class MockFetcherViewModel : FetcherViewModel
    {
        private string title;
        private MockCollectionViewModel<DocumentViewModel> documents;

        public MockFetcherViewModel()
        {
            documents = new MockCollectionViewModel<DocumentViewModel>(this);
            IsSelected = false;
            IsExpanded = false;
        }
        public MockFetcherViewModel(string title)
        {
            this.title = title;
            documents = new MockCollectionViewModel<DocumentViewModel>(this);
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
        public MockCollectionViewModel<DocumentViewModel> DesignerDocuments
        {
            get => documents;
        }
        public override ICollectionViewModel<DocumentViewModel> Documents
        {
            get => documents;
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

        public override void ChangeOwner(CategoryViewModel newViewModel)
        {
            var old = Parent as MockCategoryViewModel;
            old.SiteModelsDetail.Remove(this);
            ((MockCategoryViewModel)newViewModel).SiteModelsDetail.Add(this);
        }

        public override void AddDocument(IDocument document)
        {
            documents.Add(new MockDocumentViewModel(document));
        }
    }
}
