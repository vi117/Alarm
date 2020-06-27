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
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;

namespace ViewModel.DB
{
    class DBFetcherViewModel : FetcherViewModel
    {
        class DocumentCollection : IListViewModel<DocumentViewModel>
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private int fetcherId;

            public int Count
            {
                get
                {
                    using (var context = new AppDBContext())
                    {
                        return context.Documents.AsNoTracking().Where(x => x.DBFetcherId == fetcherId).Count();
                    }
                }
            }

            public bool IsReadOnly => true;

            public DocumentViewModel this[int index]
            {
                get
                {
                    using (var context = new AppDBContext ())
                    {
                        var dbDoc = context.Documents.AsNoTracking().Where(x => x.DBFetcherId == fetcherId).ElementAt(index);
                        return new DBDocumentViewModel(dbDoc);
                    }
                }
                set => throw new InvalidOperationException("read only");
            }

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
                        InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, doc));
                    }
                }
            }
            public void AddRange(IDocument[] dbs)
            {
                var list = new List<DocumentViewModel>();
                using(var context = new AppDBContext())
                {
                    var documents = dbs.Select(x => {
                        var ret = new DBDocument();
                        ret.SetAll(x);
                        return ret;
                    }).ToArray();
                    var fetcher = context.Fetchers.Find(fetcherId);
                    if (fetcher == null) return;
                    foreach (var i in documents)
                    {
                        var remainder = from d in context.Documents
                                        where d.DBFetcherId == fetcherId && i.GUID == d.GUID
                                        select d;
                        if (remainder.ToList().Count() == 0)
                        {
                            fetcher.Documents.Add(i);
                            list.Add(new DBDocumentViewModel(i));
                        }
                    }
                    context.SaveChanges();
                InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
                }
            }
            private IEnumerator<DocumentViewModel> getEnumerator()
            {
                using (var context = new AppDBContext())
                {
                    var ret = from doc in context.Documents.AsNoTracking()
                               where doc.DBFetcherId == fetcherId
                               orderby doc.Date descending
                               select doc;
                    return ret.ToList().Select(x=>new DBDocumentViewModel(x)).GetEnumerator();
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
                if (elem is DBDocumentViewModel doc)
                {
                    using (var context = new AppDBContext())
                    {
                        var selected = (from s in context.Documents
                                        where s.DBFetcherId == doc.FetcherId && s.GUID == doc.GUID
                                        select s);
                        context.Documents.RemoveRange(selected);
                        context.SaveChanges();
                    }
                InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,doc));
                }
                return true;
            }

            private void InvokeCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
            {
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, eventArgs);
            }

            public int IndexOf(DocumentViewModel item)
            {
                if (item is DBDocumentViewModel doc) {
                    using (var context = new AppDBContext())
                    {
                        var ddd = context.Documents.Find(doc.DocumentId);
                        return (from d in context.Documents.AsNoTracking()
                         where d.DBFetcherId == doc.FetcherId
                         select d).ToList().IndexOf(ddd);
                    }
                }
                return -1;
            }

            public void Insert(int index, DocumentViewModel item)
            {
                throw new InvalidOperationException();
            }

            public void RemoveAt(int index)
            {
                throw new InvalidOperationException();
            }

            public void Clear()
            {
                throw new InvalidOperationException();
            }

            public bool Contains(DocumentViewModel item)
            {
                if (item is DBDocumentViewModel doc)
                {
                    using (var context = new AppDBContext())
                    {
                        return (from d in context.Documents.AsNoTracking()
                                where d.DBDocumentId == doc.DocumentId
                                select d).Count() != 0;
                    }
                }
                return false;
            }

            public void CopyTo(DocumentViewModel[] array, int arrayIndex)
            {
                using (var context = new AppDBContext())
                {
                    var documents = context.Documents.AsNoTracking().Where(x => x.DBFetcherId == fetcherId).Skip(arrayIndex).Take(array.Length).ToArray();
                    documents.CopyTo(array, arrayIndex);
                }
            }
        }

        private readonly int cateogryId;
        private readonly int fetcherId;
        private string cachedTitle;
        private DocumentCollection documents;
        private Fetcher fetcher;
        private PublishedStatusCode statusCode;
        private string statusMessage;

        /// <summary>
        /// create new fetcher view model and add db.
        /// </summary>
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
                var category = context.Categories.Find(categoryId);
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
            StatusCode = args.Code;
            StatusMessage = args.DetailErrorMessage;
            if (args.Code == PublishedStatusCode.OK)
            {
                AddDocument(args.Documents.ToArray());
            }
        }
        private void AddDocument(DBDocumentViewModel dbDoc)
        {
            documents.Add(dbDoc);
        }
        private void AddDocument(IDocument[] dBDocumentViews)
        {
            documents.AddRange(dBDocumentViews);
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
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    var dbf = new DBFetcher();
                    dbf.DBFetcherId = fetcherId;
                    context.Attach(dbf);
                    dbf.DBCategoryId = dbCategory.DBCategoryId;
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                }
                old_owner_model.SitesModelDetail.CacheRemove(this);
                dbCategory.SitesModelDetail.CacheAdd(this);
            }
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
                using (var context = new AppDBContext())
                {
                    GetDBFetcher(context).SetFetcher(value);
                    context.SaveChanges();
                }
                fetcher = value;
                fetcher.OnPublished += OnPublished;
                fetcher.Start();
            }
        }

        public override IListViewModel<DocumentViewModel> Documents => documents;

        public override PublishedStatusCode StatusCode { 
            get => statusCode; 
            set {
                if (statusCode != value)
                {
                    statusCode = value;
                    OnPropertyChanged(nameof(StatusCode));
                }
            } 
        }
        public override string StatusMessage { 
            get => statusMessage;
            set
            {
                if (statusMessage != value)
                {
                    statusMessage = value;
                    OnPropertyChanged(nameof(StatusMessage));
                }
            }
        }
    }
}
