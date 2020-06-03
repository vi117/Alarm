using ViewModel.Interface;
using System.Collections.Specialized;
using Model;

namespace ViewModel
{
    public abstract class CategoryViewModel : ViewModelBase, IPageShow
    {
        public string ShowingPageName => "CategoryView";
        public object ShowingPage { get; set; }
        private bool isSelected = false;
        private bool isExpanded = false;

        abstract public void Emplace(string title,Fetcher fetcher);
        abstract public bool Remove(FetcherViewModel fetcherViewModel);
        public void RefreshAll()
        {
            foreach (var item in SiteModels)
            {
                item.Fetcher.Refresh();
            }
        }

        public abstract string Title { get; set; }
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
        public abstract ICollectionViewModel<FetcherViewModel> SiteModels { get; }
    }
}