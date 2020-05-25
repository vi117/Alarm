using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Model;
using System.Net.Cache;
using Microsoft.EntityFrameworkCore;

namespace ViewModel.DB
{
    public class DBCategoryViewModel : CategoryViewModel
    {
        public class SiteModelCollection : ICollectionViewModel<FetcherViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            List<FetcherViewModel> cache;
            int categoryId;
            ViewModelBase parent;
            DocumentPublisher publisher;
            /// <summary>
            /// For Initializing Process.
            /// Shell not be using constructor except loading.
            /// </summary>
            /// 
            /// <param name="dBCategory">Id를 가지고 있는 개체여야 한다.</param>
            /// 
            public SiteModelCollection(LoadContext context,DBCategory dBCategory, ViewModelBase parent)
            {
                this.parent = parent;
                categoryId = dBCategory.DBCategoryId;            
                cache = context.DBContext.Fetchers.Where((x)=>x.DBCategoryId == categoryId).ToList()
                    .Select((x) => (FetcherViewModel)new DBFetcherViewModel(context,x.DBFetcherId))
                    .ToList();
                publisher = context.Publisher;
            }
            public void Emplace(string title, Fetcher fetcher)
            {
                using (var dbcontext = new AppDBContext()) {
                    var context = new LoadContext() { DBContext = dbcontext, Publisher = publisher };
                    var elem = new DBFetcherViewModel(context, title, fetcher, categoryId);
                    cache.Add(elem);
                }
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            public void Add(FetcherViewModel elem)
            {
                elem.Parent = parent;
                cache.Add(elem);
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            public IEnumerator<FetcherViewModel> GetEnumerator()
            {
                return cache.GetEnumerator();
            }

            public bool Remove(FetcherViewModel elem)
            {
                if (elem is DBFetcherViewModel fetcherViewModel)
                {
                    using (var context = new AppDBContext())
                    {
                        context.Categorys.Find(categoryId)
                            .Fetchers.Remove(fetcherViewModel.GetFetcher(context));
                    }
                }
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return cache.Remove(elem);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return cache.GetEnumerator();
            }
        }
        private int categoryId;
        private SiteModelCollection siteModels;
        /// <summary>
        /// if <c>category</c> not in db, create and add it, else find and initialize it.
        /// </summary>
        /// <param name="category"></param>
        public DBCategoryViewModel(LoadContext context, DBCategory category)
        {
            if (!context.DBContext.Categorys.Contains(category))
            {
                context.DBContext.Add(category);
                Title = category.Title;
                context.DBContext.SaveChanges();
                categoryId = category.DBCategoryId;
            }
            else
            {
                Title = category.Title;
                categoryId = category.DBCategoryId;
            }
            siteModels = new SiteModelCollection(context,category, this);
        }
        public DBCategoryViewModel()
        {
        }
        
        public override string Title {
            get; set;
        }
        public DBCategory GetDBCategory(AppDBContext context) {
            return context.Categorys.Find(categoryId);
        }
        public SiteModelCollection SitesModelDetail => siteModels;
        public override ICollectionViewModel<FetcherViewModel> SiteModels {
            get => siteModels;
        }
    }
}
