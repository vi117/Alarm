using ViewModel.Interface;
using System.ComponentModel;

namespace ViewModel
{
    public abstract class ViewModelBase : IViewModelBase
    {
        /// <summary>
        /// It refer to ViewModel which have the collection containing <c>this</c>.
        /// </summary>
        public IViewModelBase Parent { get; set; }

        public ViewModelBase() { Parent = null; }
        public ViewModelBase(IViewModelBase parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Fire the <c>PropertyChangedEvent</c>.
        /// </summary>
        /// <param name="propertyName">changed <paramref name="propertyName"/></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PlatformSevice.Instance.PropertyChangedInvoke(this, PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
