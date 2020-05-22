using Alarm.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace Alarm.ViewModels
{
    public class CollectionViewModel<T> : ObservableCollection<T> where T : IViewModelBase
    {
        public IViewModelBase Parent { get; set; }
        public readonly object colLock = new object();
        //for designer mode
        public CollectionViewModel()
        {
            Parent = null;
            BindingOperations.EnableCollectionSynchronization(this, colLock);
        }
        public CollectionViewModel(IViewModelBase parent)
        {
            this.Parent = parent;
        }
        /*protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }*/
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
            NotifyCollectionChangedEventHandler CollectionChanged = this.CollectionChanged;
            if (CollectionChanged == null) return;
            foreach (NotifyCollectionChangedEventHandler nh in CollectionChanged.GetInvocationList())
            {
                DispatcherObject dispObj = nh.Target as DispatcherObject;
                if (dispObj != null)
                {
                    Dispatcher dispatcher = dispObj.Dispatcher; //그 쓰레드의 디스패처에 접근해서
                    if (dispatcher != null && !dispatcher.CheckAccess()) // 액세스 불가능하면
                    {
                        dispatcher.BeginInvoke(//STA thread의 디스패처에게 가서 이벤트를 보냄.
                            (Action)(() => nh.Invoke(this,
                               e)),// new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                            DispatcherPriority.DataBind);
                        continue;
                    }
                }
                nh.Invoke(this, e);
            }
        }
    }
}
