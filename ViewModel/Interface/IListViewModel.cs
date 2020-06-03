using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ViewModel.Interface
{
    public interface IListViewModel<T> : INotifyCollectionChanged, IList<T>, ICollection<T>
    {
    }
}
