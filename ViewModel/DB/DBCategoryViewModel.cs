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
                    .Select((x) => (FetcherViewModel)new DBFetcherViewModel(context,x.DBFetcherId,parent))
                    .ToList();
                publisher = context.Publisher;
            }
            public SiteModelCollection(DocumentPublisher publisher, DBCategory category, ViewModelBase parent)
            {
                this.parent = parent;
                this.publisher = publisher;
                categoryId = category.DBCategoryId;
                cache = new List<FetcherViewModel>();
            }
            public void Emplace(string title, Fetcher fetcher)
            {
                var elem = new DBFetcherViewModel(publisher, title, fetcher, categoryId);
                elem.Parent = parent;
                cache.Add(elem);
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            public void CacheAdd(FetcherViewModel elem)
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

            public bool CacheRemove(FetcherViewModel elem)
            {
                bool ret = cache.Remove(elem);
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return ret;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return cache.GetEnumerator();
            }
        }
        private int categoryId;
        private SiteModelCollection siteModels;
        /// <summary>
        /// Load only.
        /// </summary>
        /// <param name="category"></param>
        public DBCategoryViewModel(LoadContext context, DBCategory category,ViewModelBase parent)
        {
            this.Parent = parent;
            Title = category.Title;
            categoryId = category.DBCategoryId;
            siteModels = new SiteModelCollection(context,category, this);
        }

        public DBCategoryViewModel(string title, DocumentPublisher publisher)
        {
            Title = title;
            using (var context = new AppDBContext()) {
                var dbCategory = new DBCategory() { Title = title };
                context.Categorys.Add(dbCategory);
                context.SaveChanges();
                siteModels = new SiteModelCollection(publisher, dbCategory, this);
            }
        }
        
        public override string Title {
            get; set;
        }
        public DBCategory GetDBCategory(AppDBContext context) {
            return context.Categorys.Find(categoryId);
        }

        public override void Emplace(string title, Fetcher fetcher)
        {
            siteModels.Emplace(title, fetcher);
        }

        public override bool Remove(FetcherViewModel fetcherViewModel)
        {
            bool ret = siteModels.CacheRemove(fetcherViewModel);
            if (fetcherViewModel is DBFetcherViewModel dbFetcherViewModel)
            {
                using (var context = new AppDBContext())
                {
                    var dbfetcher = dbFetcherViewModel.GetDBFetcher(context);
                    var dbCategory = GetDBCategory(context);
                    dbCategory.Fetchers.Remove(dbfetcher);
                    context.SaveChanges();
                }
            }
            return ret;
        }

        public SiteModelCollection SitesModelDetail => siteModels;
        public override ICollectionViewModel<FetcherViewModel> SiteModels {
            get => siteModels;
        }
    }
}
