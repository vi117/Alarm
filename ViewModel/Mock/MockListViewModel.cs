using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using ViewModel.Interface;

namespace ViewModel
{
    public class MockListViewModel<T> : IListViewModel<T> where T : ViewModelBase
    {
        private List<T> ts;
        ViewModelBase parent;
        public MockListViewModel(ViewModelBase parent)
        {
            ts = new List<T>();
            this.parent = parent;
        }

        public T this[int index] { get => ts[index]; 
            set {
                ts[index] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,value));
            } 
        }

        public int Count => ts.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            PlatformSevice.Instance.CollectionChangedInvoke(this, this.CollectionChanged, e);
        }

        public void Add(T item)
        {
            ts.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            ts.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return ts.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ts.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ts.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ts.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            ts.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public bool Remove(T item)
        {
            bool ret = ts.Remove(item);
            if(ret) OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return ret;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            ts.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ts.GetEnumerator();
        }
    }
}
