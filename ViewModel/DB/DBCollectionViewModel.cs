using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    public interface Converter<M, T> {
        M From(T arg);
        T From(M arg);
    }
    public class DBCollectionViewModel<M,T> : CollectionViewModel<T> where T : ViewModelBase
    {
        Converter<M, T> converter;
        private List<M> target;
        public DBCollectionViewModel(List<M> target, Converter<M,T> converter)
            :base(target.Select((x)=>converter.From(x)).ToList())
        {
            this.converter = converter;
            this.target = target;
        }
        protected override void InsertItem(int index, T item)
        {
            var context = new AppDBContext();
            base.InsertItem(index, item);
            target.Insert(index, converter.From(item));
            context.SaveChanges();
        }
        protected override void ClearItems()
        {
            var context = new AppDBContext();
            base.ClearItems();
            target.Clear();
            context.SaveChanges();
        }
        protected override void RemoveItem(int index)
        {
            var context = new AppDBContext();
            base.RemoveItem(index);
            target.RemoveAt(index);
            context.SaveChanges();
        }
        protected override void SetItem(int index, T item)
        {
            var context = new AppDBContext();
            base.SetItem(index, item);
            target[index] = converter.From(item);
            context.SaveChanges();
        }
    }
}
