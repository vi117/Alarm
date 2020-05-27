using ViewModel.Interface;

namespace ViewModel
{
    abstract public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private object displayPage;
        public ViewModel()
        {
            Parent = null;
        }

        public abstract void EmplaceCategory(string title);

        #region Property
        abstract public ICollectionViewModel<CategoryViewModel> TreeView { get; }

        public object DisplayPage
        {
            get => displayPage;
            set
            {
                if (displayPage != value)
                {
                    displayPage = value;
                    OnPropertyChanged(nameof(displayPage));
                }
            }
        }
        #endregion

        //Navigate page
        public void Navigate(IPageShow model , IPageFactory pageFactory)
        {
            var page = pageFactory.Generate(model);
            DisplayPage = page;
        }
    }
}
