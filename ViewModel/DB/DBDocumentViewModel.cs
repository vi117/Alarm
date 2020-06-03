using Model.DB;
using Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    class DBDocumentViewModel : DocumentViewModel
    {
        private int documentId;
        private int fetcherId;

        bool isRead;
        /// <summary>
        /// Read
        /// </summary>
        public DBDocumentViewModel(DBDocument dBDocument)
        {
            documentId = dBDocument.DBDocumentId;
            fetcherId = dBDocument.DBFetcherId;
            this.SetAll(dBDocument);
            isRead = dBDocument.IsRead;
        }
        /// <summary>
        /// Only for inserting document.
        /// </summary>
        public DBDocumentViewModel(IDocument document)
        {
            documentId = 0;
            fetcherId = 0;
            this.SetAll(document);
            isRead = false;
        }
        public int DocumentId => documentId;
        public int FetcherId => fetcherId;
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
                    OnPropertyChanged(nameof(IsRead));
                }
            }
        }

        public override object ShowingPage {
            get => base.ShowingPage;
            set { 
                //If it show the page, set IsRead to true.
                base.ShowingPage = value;
                IsRead = true;
            }
        }
    }
}
