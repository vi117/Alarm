using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ViewModel.DB
{
    public class DBViewModel : ViewModel
    {
        class TreeViewCollection : ICollectionViewModel<CategoryViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private Dictionary<string,CategoryViewModel> categoriesCache;
            private ViewModelBase parent;

            public TreeViewCollection(ViewModelBase parent) {
                this.parent = parent;
                using (var context = new AppDBContext()){
                    categoriesCache =
                        context.Categorys.ToList()
                        .Select((x) => (CategoryViewModel)new DBCategoryViewModel(x))
                        .OrderBy((x) => x.Title)
                        .ToDictionary((x) => x.Title);
                }
            }

            public void Add(CategoryViewModel elem)
            {
                elem.Parent = parent;
                if (elem is DBCategoryViewModel dB)
                {
                    categoriesCache.Add(dB.Title, dB);
                }
                else throw new NotImplementedException();
            }

            public IEnumerator<CategoryViewModel> GetEnumerator()
            {
                return categoriesCache.Values.GetEnumerator();
            }

            public bool Remove(CategoryViewModel elem)
            {
                if (elem is DBCategoryViewModel dB)
                {
                    using (var context = new AppDBContext())
                    {
                        context.Categorys.Remove(dB.GetDBCategory(context));
                    }
                }
                return categoriesCache.Remove(elem.Title);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return categoriesCache.Values.GetEnumerator();
            }
        }
        private TreeViewCollection categories;

        public DBViewModel()
        {
            categories = new TreeViewCollection(this);
        }

        public override ICollectionViewModel<CategoryViewModel> TreeView { 
            get {
                return categories;
            }
        }
    }
}
