using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Windows.Controls;
using Model.Interface;

namespace Alarm.ViewModels
{
    public class MockDocumentViewModel : DocumentViewModel
    {
        public override string Title { get; set; }

        public override string HostUri { get; set; }
        public override string PathUri { get; set; }
        public override string Summary { get; set; }
        public override DateTime Date { get; set; }
        public override string GUID { get; set; }

        public MockDocumentViewModel() : base()
        { }
        public MockDocumentViewModel(IDocument document) : base()
        {
            HostUri = document.HostUri;
            PathUri = document.PathUri;
            Date = document.Date;
            GUID = document.GUID;
            Summary = document.Summary;
            Title = document.Title;
        }

        public bool IsRead { get; set; } = false;
    }
}
