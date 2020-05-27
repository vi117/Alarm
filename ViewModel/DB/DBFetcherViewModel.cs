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
                           orderby doc.Date descending
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

        /// <summary>
        /// create new fetcher view model and add db.
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="title"></param>
        /// <param name="fetcher"></param>
        /// <param name="categoryId"></param>
        public DBFetcherViewModel(DocumentPublisher publisher, string title, Fetcher fetcher, int categoryId)
        {
            using (var context = new AppDBContext())
            {
                Title = title;
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
                publisher.AddFetcher(fetcher, OnPublished);
            }
        }

        /// <summary>
        /// Load From DB
        /// </summary>
        public DBFetcherViewModel(LoadContext context, int fetcherId, ViewModelBase parent)
        {
            Parent = parent;
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
            if(newViewModel is DBCategoryViewModel dbCategory)
            {
                using (var context = new AppDBContext())
                {
                    var old_owner_model = Parent as DBCategoryViewModel;
                    var new_owner = dbCategory.GetDBCategory(context);
                    GetDBFetcher(context).ChangeOwner(new_owner);
                    old_owner_model.SitesModelDetail.CacheRemove(this);
                    dbCategory.SitesModelDetail.CacheAdd(this);
                    context.SaveChanges();
                }
            }
        }


        public override ICollectionViewModel<DocumentViewModel> Documents { 
            get => documents; 
        }
        public override string Title {
            get; set;
        }
    }
}
