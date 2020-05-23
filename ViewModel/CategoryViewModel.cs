using ViewModel.Interface;
using System.Collections.Specialized;


namespace ViewModel
{
    public abstract class CategoryViewModel : ViewModelBase, IPageShow
    {
        public string ShowingPageName => "CategoryView";
        public object ShowingPage { get; set; }
        private bool isSelected = false;
        private bool isExpanded = false;


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
        public abstract string Title { get; set; }
    }
}