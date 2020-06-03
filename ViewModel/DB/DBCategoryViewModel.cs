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
                cache = context.DBContext.Fetchers.AsNoTracking().Where((x)=>x.DBCategoryId == categoryId).ToArray()
                    .Select((x) => (FetcherViewModel)new DBFetcherViewModel(context,x.DBFetcherId,parent))
                    .ToList();
            }
            public SiteModelCollection(DBCategory category, ViewModelBase parent)
            {
                this.parent = parent;
                categoryId = category.DBCategoryId;
                cache = new List<FetcherViewModel>();
            }
            public void Emplace(string title, Fetcher fetcher)
            {
                var elem = new DBFetcherViewModel(title, fetcher, categoryId);
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
        private string cachedTitle;
        private SiteModelCollection siteModels;
        /// <summary>
        /// Load only.
        /// </summary>
        /// <param name="category"></param>
        public DBCategoryViewModel(LoadContext context, DBCategory category,ViewModelBase parent)
        {
            this.Parent = parent;
            cachedTitle = category.Title;
            categoryId = category.DBCategoryId;
            siteModels = new SiteModelCollection(context,category, this);
        }

        public DBCategoryViewModel(string title)
        {
            cachedTitle = title;
            using (var context = new AppDBContext()) {
                var dbCategory = new DBCategory() { Title = title };
                context.Categorys.Add(dbCategory);
                context.SaveChanges();
                siteModels = new SiteModelCollection(dbCategory, this);
            }
        }
        
        public override string Title {
            get => cachedTitle;
            set
            {
                using(var context =new AppDBContext())
                {
                    GetDBCategory(context).Title = value;
                    context.SaveChanges();
                }
                cachedTitle = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public DBCategory GetDBCategory(AppDBContext context) {
            return context.Categorys.Find(categoryId);
        }
        public int DBCategoryId => categoryId;

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
