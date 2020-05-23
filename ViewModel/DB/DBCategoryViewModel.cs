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

namespace ViewModel.DB
{
    class DBCategoryViewModel : CategoryViewModel
    {
        public class SiteModelCollection : ICollectionViewModel<FetcherViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            List<FetcherViewModel> cache;
            int categoryId;
            ViewModelBase parent;

            public SiteModelCollection(DBCategory dBCategory, ViewModelBase parent)
            {
                this.parent = parent;
                categoryId = dBCategory.DBCategoryId;
                cache = dBCategory.Fetchers.ToList()
                    .Select((x) => (FetcherViewModel)new DBFetcherViewModel(x.DBFetcherId))
                    .ToList();
            }
            public void Emplace(string title, Fetcher fetcher)
            {
                var elem = new DBFetcherViewModel(title, fetcher, categoryId);
                cache.Add(elem);
            }
            public void Add(FetcherViewModel elem)
            {
                elem.Parent = parent;
                cache.Add(elem);
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
        public DBCategoryViewModel(DBCategory category)
        {
            using(var context=new AppDBContext())
            {
                if (!context.Categorys.Contains(category))
                {
                    context.Add(category);
                    Title = category.Title;
                    context.SaveChanges();
                    categoryId = category.DBCategoryId;
                }
                else
                {
                    Title = category.Title;
                    categoryId = category.DBCategoryId;
                }
            }
        }
        public override string Title {
            get;
            set;
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
