using ViewModel.Interface;

namespace ViewModel
{
    /// <summary>
    /// ViewModel of All
    /// </summary>
    abstract public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private object displayPage;
        public ViewModel()
        {
            Parent = null;
        }
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
        //Navigate page
        public void Navigate(IPageShow model , IPageFactory pageFactory)
        {
            var page = pageFactory.Generate(model);
            DisplayPage = page;
        }
    }
}
