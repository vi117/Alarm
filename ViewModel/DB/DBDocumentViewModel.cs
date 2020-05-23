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
        private DBDocument document;

        public DBDocumentViewModel(DBDocument document) {
            this.document = document;
        }

        public override string Title { get => document.Title; set => throw new NotImplementedException(); }
        public override string HostUri { get => document.HostUri; set => throw new NotImplementedException(); }
        public override string PathUri { get => document.PathUri; set => throw new NotImplementedException(); }
        public override string Summary { get => document.Summary; set => throw new NotImplementedException(); }
        public override DateTime Date { get => document.Date; set => throw new NotImplementedException(); }
        public override string GUID { get => document.GUID; set => throw new NotImplementedException(); }
    }
}
