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
                List<DBDocumentViewModel> ret;
                using (var context = new AppDBContext())
                {
                    ret = (from doc in context.Documents
                           where doc.DBFetcherId == fetcherId
                           select new DBDocumentViewModel(doc)).ToList();
                }
                return ret.GetEnumerator();
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
                using (var context= new AppDBContext())
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
