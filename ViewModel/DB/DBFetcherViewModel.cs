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
                    var fetcher = GetDBFetcher(context);
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
                        fetcher.Documents.Add(doc);
                    }
                }
            }

            public IEnumerator<DocumentViewModel> GetEnumerator()
            {
                using(var context = new AppDBContext())
                {
                    var fetcher = GetDBFetcher(context);
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
                return true;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                using (var context = new AppDBContext())
                {
                    var fetcher = GetDBFetcher(context);
                    return fetcher.Documents
                        .Select((x) => new DBDocumentViewModel(x))
                        .GetEnumerator();
                }
            }
        }

        private int cateogryId;
        private int fetcherId;
        private DocumentCollection documents;

        public DBFetcherViewModel(string title,Fetcher fetcher,int categoryId)
        {
            using (var context = new AppDBContext())
            {
                Title = title;
                var f = new DBFetcher() {
                    Title = title
                };
                f.SetFetcher(fetcher);
                context.Categorys.Find(categoryId)
                    .Fetchers.Add(f);
                context.SaveChanges();
                fetcherId = f.DBFetcherId;
            }
            documents = new DocumentCollection(fetcherId);
            cateogryId = categoryId;
        }
        public DBFetcherViewModel(int fetcherId)
        {
            this.fetcherId = fetcherId;
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
