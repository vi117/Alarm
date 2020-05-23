using ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CollectionViewModel<T> : ObservableCollection<T> , ICollectionViewModel<T> where T : IViewModelBase
    {
        public IViewModelBase Parent { get; set; }
        public readonly object colLock = new object();
        //for designer mode
        public CollectionViewModel()
        {
            Parent = null;
            PlatformSevice.Instance.EnableCollectionSynchronization(this, colLock);
        }
        public CollectionViewModel(IViewModelBase parent)
        {
            this.Parent = parent;
            PlatformSevice.Instance.EnableCollectionSynchronization(this, colLock);
        }
        public CollectionViewModel(List<T> ts) : base(ts) { this.Parent = null; }

        protected override void InsertItem(int index, T item)
        {
            item.Parent = Parent;
            base.InsertItem(index, item);
        }
        protected override void SetItem(int index, T item)
        {
            item.Parent = Parent;
            base.SetItem(index, item);
        }
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            PlatformSevice.Instance.CollectionChangedInvoke(this,this.CollectionChanged, e);
        }
    }
}
