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
                    var fetcher = context.Fetchers.Find(fetcherId);
                    if (fetcher.Documents.Where((x) => x.GUID == elem.GUID).Count() == 0)
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
                        fetcher = context.Fetchers.Find(fetcherId);
                        fetcher.Documents.Add(doc);
                        //context.Documents.Add(doc);
                        context.SaveChanges();
                    }
                }
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            public IEnumerator<DocumentViewModel> GetEnumerator()
            {
                using(var context = new AppDBContext())
                {
                    var fetcher = context.Fetchers.Find(fetcherId);

                    return fetcher.Documents
                        .Select((x) => new DBDocumentViewModel(x))
                        .GetEnumerator();
                }
            }

            public bool Remove(DocumentViewModel elem)
            {
                using (var context= new AppDBContext())
                {
                    var fetcher = GetDBFetcher(context);
                    var selected = fetcher.Documents.Where((x) => x.GUID == elem.GUID).ToList();
                    if (selected.Count == 0) return false;
                    foreach (var s in selected)
                    {
                        fetcher.Documents.Remove(s);
                    }
                    context.SaveChanges();
                }
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return true;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                List<DBDocumentViewModel> ret;
                using (var context = new AppDBContext())
                {
                    /*var fetcher = context.Fetchers.Find(fetcherId);
                    ret = fetcher.Documents.ToList()
                        .Select((x) => new DBDocumentViewModel(x))
                        .ToList();*/
                    ret = context.Documents.Where((x) => x.DBFetcher.DBFetcherId == fetcherId).ToList()
                        .Select((x) => new DBDocumentViewModel(x))
                        .ToList();
                }
                return ret.GetEnumerator();
            }
            internal void Invoke()
            {
                PlatformSevice.Instance.CollectionChangedInvoke
                    (this, this.CollectionChanged, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private int cateogryId;
        private int fetcherId;
        private DocumentCollection documents;

        public DBFetcherViewModel(LoadContext context, string title, Fetcher fetcher, int categoryId)
        {
            Title = title;
            var f = new DBFetcher()
            {
                Title = title
            };
            f.SetFetcher(fetcher);
            var category = context.DBContext.Categorys.Find(categoryId);
            category.Fetchers.Add(f);
            context.DBContext.SaveChanges();
            fetcherId = f.DBFetcherId;
            cateogryId = categoryId;
            documents = new DocumentCollection(fetcherId);
            context.Publisher.AddFetcher(fetcher, OnPublished);
        }
        public DBFetcherViewModel(LoadContext context, int fetcherId)
        {
            var dbfetcher = context.DBContext.Fetchers.Find(fetcherId);
            Title = dbfetcher.Title;
            var fetcher = dbfetcher.GetFetcher();
            this.fetcherId = fetcherId;
            context.Publisher.AddFetcher(
                fetcher,
                OnPublished
            );
            documents = new DocumentCollection(fetcherId);
        }
        private void OnPublished(object sender, PublishedEventArg args)
        {
            //using (var dBContext = new AppDBContext())
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
                    //dBContext.Fetchers.Find(fetcherId).Documents.Add(dbDoc);
                    Documents.Add(new DBDocumentViewModel(dbDoc));
                    //dBContext.SaveChanges();
                }
            }
        }
        public DBFetcher GetFetcher(AppDBContext context)
        {
            return context.Fetchers.Find(fetcherId);
        }

        public override ICollectionViewModel<DocumentViewModel> Documents { 
            get => documents; 
        }
        public override string Title {
            get; set;
        }
    }
}
