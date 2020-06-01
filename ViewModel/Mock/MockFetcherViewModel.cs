using ViewModel.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Interface;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class MockFetcherViewModel : FetcherViewModel
    {
        //알수 없는 에러로 인해.
        public class DesignHelper : ObservableCollection<MockDocumentViewModel>
        {
            MockListViewModel<DocumentViewModel> documentViews;
            public DesignHelper(MockListViewModel<DocumentViewModel> d) {
                documentViews = d;
            }
            protected override void InsertItem(int index, MockDocumentViewModel item)
            {
                base.InsertItem(index, item);
                documentViews.Add(item);
            }
        }
        private string title;
        private MockListViewModel<DocumentViewModel> documents;
        private Fetcher fetcher;

        public MockFetcherViewModel():this(""){}

        public MockFetcherViewModel(string title):this(title,new MockFetcher()){}

        public MockFetcherViewModel(string title,Fetcher fetcher)
        {
            this.fetcher = fetcher;
            this.title = title;
            documents = new MockListViewModel<DocumentViewModel>(this);
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
        public DesignHelper DesignerDocuments => new DesignHelper(documents);
        
        public override IListViewModel<DocumentViewModel> Documents => documents;
        public override Fetcher Fetcher { 
            get => fetcher;
            set => fetcher = value; 
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
