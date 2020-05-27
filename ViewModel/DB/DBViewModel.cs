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
using Model;

namespace ViewModel.DB
{
    public class DBViewModel : ViewModel
    {
        class TreeViewCollection : ICollectionViewModel<CategoryViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private Dictionary<string,CategoryViewModel> categoriesCache;
            private ViewModelBase parent;
            private DocumentPublisher documentPublisher;
            /// <summary>
            /// For Initializing Process.
            /// Shell not be using constructor except loading.
            /// </summary>
            /// <param name="parent"></param>
            public TreeViewCollection(LoadContext loadContext, ViewModelBase parent) {
                this.parent = parent;
                this.documentPublisher = loadContext.Publisher;
                categoriesCache =
                        loadContext.DBContext.Categorys.ToList()
                        .Select((x) => (CategoryViewModel)new DBCategoryViewModel(loadContext, x, parent))
                        .OrderBy((x) => x.Title)
                        .ToDictionary((x) => x.Title);
                if(categoriesCache.Count == 0)
                {
                    Add(new DBCategoryViewModel
                            (
                                loadContext,
                                new DBCategory() { Title = "Default" },
                                parent
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
            public void Emplace(string title)
            {
                var dBCategoryView = new DBCategoryViewModel(title,documentPublisher);
                categoriesCache.Add(title, dBCategoryView);
                dBCategoryView.Parent = parent;
                PlatformSevice.Instance.CollectionChangedInvoke
                        (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            private IEnumerator<CategoryViewModel> getEnumerator()
            {
                return (from kv in categoriesCache
                        orderby kv.Key
                        select kv.Value).GetEnumerator();
            }
            public IEnumerator<CategoryViewModel> GetEnumerator()
            {
                return getEnumerator();
            }

            public bool Remove(CategoryViewModel elem)
            {
                if (elem is DBCategoryViewModel dB)
                {
                    using (var context = new AppDBContext())
                    {
                        context.Categorys.Remove(dB.GetDBCategory(context));
                        context.SaveChanges();
                    }
                }
                bool ret = categoriesCache.Remove(elem.Title);
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return ret;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return getEnumerator();
            }
        }
        private TreeViewCollection categories;

        public override void EmplaceCategory(string title)
        {
            categories.Emplace(title);
        }

        public override bool RemoveCategory(CategoryViewModel categoryViewModel)
        {
            return categories.Remove(categoryViewModel);
        }

        //Load Only
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
