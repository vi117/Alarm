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

            /// <summary>
            /// For Initializing Process.
            /// Shell not be using constructor except loading.
            /// </summary>
            /// <param name="parent"></param>
            public TreeViewCollection(LoadContext loadContext, ViewModelBase parent) {
                this.parent = parent;
                categoriesCache =
                        loadContext.DBContext.Categorys.ToList()
                        .Select((x) => (CategoryViewModel)new DBCategoryViewModel(loadContext, x))
                        .OrderBy((x) => x.Title)
                        .ToDictionary((x) => x.Title);
                if(categoriesCache.Count == 0)
                {
                    Add(new DBCategoryViewModel
                            (
                                loadContext,
                                new DBCategory() { Title = "Default" }
                            )
                        );
                    loadContext.DBContext.SaveChanges();
                }
            }

            public void Add(CategoryViewModel elem)
            {
                elem.Parent = parent;
                if (elem is DBCategoryViewModel dB)
                {
                    categoriesCache.Add(dB.Title, dB);
                    PlatformSevice.Instance.CollectionChangedInvoke
                        (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return categoriesCache.Remove(elem.Title);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return categoriesCache.Values.GetEnumerator();
            }
        }
        private TreeViewCollection categories;

        public DBViewModel(LoadContext loadContext)
        {
            categories = new TreeViewCollection(loadContext, this);
        }

        public override ICollectionViewModel<CategoryViewModel> TreeView { 
            get {
                return categories;
            }
        }
    }
}
