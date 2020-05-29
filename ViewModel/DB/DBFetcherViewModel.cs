using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Collections.Specialized;
using System.Collections;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ViewModel.DB
{
    class DBFetcherViewModel : FetcherViewModel
    {
        class DocumentCollection : ICollectionViewModel<DocumentViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private int fetcherId;
            public DocumentCollection(int id)
            {
                fetcherId = id;
            }
            public DBFetcher GetDBFetcher(AppDBContext context)
            {
                return context.Fetchers.Find(fetcherId);
            }
            public void Add(DocumentViewModel elem)
            {
                using (var context = new AppDBContext())
                {
                    if ((from s in context.Documents
                         where s.DBFetcherId == fetcherId && s.GUID == elem.GUID
                         select s
                         ).Count() == 0
                         )
                    {
                        var doc = new DBDocument()
                        {
                            Title = elem.Title,
                            Summary = elem.Summary,
                            Date = elem.Date,
                            GUID = elem.GUID,
                            HostUri = elem.HostUri,
                            PathUri = elem.PathUri,
                            IsRead = elem.IsRead,
                        };
                        var fetcher = context.Fetchers.Find(fetcherId);
                        fetcher.Documents.Add(doc);
                        context.SaveChanges();
                    }
                }
                InvokeCollectionChanged();
            }
            private IEnumerator<DocumentViewModel> getEnumerator()
            {
                using (var context = new AppDBContext())
                {
                    var ret = (from doc in context.Documents
                               where doc.DBFetcherId == fetcherId
                               orderby doc.Date descending
                               select new DBDocumentViewModel(doc));
                    return ret.ToList().GetEnumerator();
                }
            }
            public IEnumerator<DocumentViewModel> GetEnumerator()
            {
                return getEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return getEnumerator();
            }

            public bool Remove(DocumentViewModel elem)
            {
                using (var context = new AppDBContext())
                {
                    var selected = (from s in context.Documents
                                    where s.GUID == elem.GUID
                                    select s);
                    if (selected.Count() == 0) return false;
                    foreach (var s in selected)
                    {
                        context.Documents.Remove(s);
                    }
                    context.SaveChanges();
                }
                InvokeCollectionChanged();
                return true;
            }

            private void InvokeCollectionChanged()
            {
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private int cateogryId;
        private int fetcherId;
        private string cachedTitle;
        private DocumentCollection documents;
        private Fetcher fetcher;

        /// <summary>
        /// create new fetcher view model and add db.
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="title"></param>
        /// <param name="fetcher"></param>
        /// <param name="categoryId"></param>
        public DBFetcherViewModel(string title, Fetcher fetcher, int categoryId)
        {
            this.fetcher = fetcher;
            using (var context = new AppDBContext())
            {
                cachedTitle = title;
                var f = new DBFetcher()
                {
                    Title = title
                };
                f.SetFetcher(fetcher);
                var category = context.Categorys.Find(categoryId);
                category.Fetchers.Add(f);
                context.SaveChanges();
                fetcherId = f.DBFetcherId;
                this.cateogryId = categoryId;
                documents = new DocumentCollection(fetcherId);
                fetcher.OnPublished += OnPublished;
                fetcher.Start();
            }
        }

        /// <summary>
        /// Load From DB
        /// </summary>
        public DBFetcherViewModel(LoadContext context, int fetcherId, ViewModelBase parent)
        {
            Parent = parent;
            var dbfetcher = context.DBContext.Fetchers.Find(fetcherId);
            cachedTitle = dbfetcher.Title;
            fetcher = dbfetcher.GetFetcher();
            this.fetcherId = fetcherId;
            fetcher.OnPublished += OnPublished;
            fetcher.Start();
            documents = new DocumentCollection(fetcherId);
        }
        private void OnPublished(object sender, PublishedEventArg args)
        {
            foreach (var doc in args.Documents)
            {
                var dbDoc = new DBDocument()
                {
                    Title = doc.Title,
                    Summary = doc.Summary,
                    Date = doc.Date,
                    HostUri = doc.HostUri,
                    PathUri = doc.PathUri,
                    GUID = doc.GUID,
                    IsRead = false
                };
                AddDocument(new DBDocumentViewModel(dbDoc));
            }
        }
        private void AddDocument(DBDocumentViewModel dbDoc)
        {
            documents.Add(dbDoc);
        }
        public override void AddDocument(IDocument document)
        {
            documents.Add(new DBDocumentViewModel(document));
        }
        public DBFetcher GetDBFetcher(AppDBContext context)
        {
            return context.Fetchers.Find(fetcherId);
        }

        public override void ChangeOwner(CategoryViewModel newViewModel)
        {
            if (newViewModel is DBCategoryViewModel dbCategory)
            {
                var old_owner_model = Parent as DBCategoryViewModel;
                using (var context = new AppDBContext())
                {
                    var dbf = new DBFetcher();
                    dbf.DBFetcherId = fetcherId;
                    context.Attach(dbf);
                    dbf.DBCategoryId = dbCategory.DBCategoryId;
                    //For speed
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    context.SaveChanges();
                }
                old_owner_model.SitesModelDetail.CacheRemove(this);
                dbCategory.SitesModelDetail.CacheAdd(this);
            }
        }


        public override ICollectionViewModel<DocumentViewModel> Documents
        {
            get => documents;
        }
        public override string Title
        {
            get => cachedTitle;
            set
            {
                using (var context = new AppDBContext())
                {
                    GetDBFetcher(context).Title = value;
                    context.SaveChanges();
                }
                cachedTitle = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public override Fetcher Fetcher
        {
            get => fetcher;
            set
            {
                fetcher.OnPublished -= OnPublished;
                fetcher.Stop();
                using(var context = new AppDBContext())
                {
                    GetDBFetcher(context).SetFetcher(value);
                    context.SaveChanges();
                }
                fetcher = value;
                fetcher.OnPublished += OnPublished;
                fetcher.Start();
            }
        }
    }
}
