namespace ViewModel.Interface
{
    public interface IViewModelBehavior
    {
        void Navigate(IPageShow page, IPageFactory pageFactory);
    }
}
