using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    class DBDocumentViewModel : DocumentViewModel
    {
        private int documentId;

        bool isRead;

        public DBDocumentViewModel(DBDocument dBDocument)
        {
            documentId = dBDocument.DBDocumentId;
            Title = dBDocument.Title;
            HostUri = dBDocument.HostUri;
            PathUri = dBDocument.PathUri;
            Summary = dBDocument.Summary;
            Date = dBDocument.Date;
            GUID = dBDocument.GUID;
            isRead = dBDocument.IsRead;
        }
        public int DocumentId { get => documentId; }
        public override string Title {
            get; set;
        }
        public override string HostUri {
            get; set;
        }
        public override string PathUri { 
            get; set; 
        }
        public override string Summary { 
            get; set; 
        }
        public override DateTime Date { 
            get; set; 
        }
        public override string GUID {
            get; set;
        }
        public override bool IsRead { 
            get => isRead;
            set{
                if(value && !isRead)
                {
                    using(var context = new AppDBContext())
                    {
                        context.Documents.Find(documentId).IsRead = true;
                        isRead = true;
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
