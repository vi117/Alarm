using ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Interface;
using Model;

namespace ViewModel
{
    public abstract class FetcherViewModel : ViewModelBase, IPageShow
    {

        public string ShowingPageName => "ContentListView";
        public object ShowingPage { get; set; }

        public FetcherViewModel()
        {
            Parent = null;
        }

        abstract public void AddDocument(IDocument document);
        abstract public void ChangeOwner(CategoryViewModel newViewModel);
        public void Refresh()
        {
            Fetcher.Refresh();
        }
        public abstract string Title { get; set; }
        public abstract PublishedStatusCode StatusCode { get; set; }
        public abstract string StatusMessage { get; set; }

        public abstract Fetcher Fetcher { get; set; }
        public abstract IListViewModel<DocumentViewModel> Documents { get; }
    }
}
