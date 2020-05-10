using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels
{
    public class CollectionViewModel<T> : ObservableCollection<T> where T : ViewModelBase
    {
        private IViewModelBehavior modelBehavior;
        //for designer mode
        public CollectionViewModel()
        {
        }
        public CollectionViewModel(IViewModelBehavior modelBehavior)
        {
            this.modelBehavior = modelBehavior;
        }
        protected override void InsertItem(int index, T item)
        {
            item.Root = modelBehavior;
            base.InsertItem(index, item);
        }
        protected override void SetItem(int index, T item)
        {
            item.Root = modelBehavior;
            base.SetItem(index, item);
        }
    }
}
