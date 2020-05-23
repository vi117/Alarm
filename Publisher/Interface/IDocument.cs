using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interface
{
    public interface IDocument
    {
        string Title { get; set; }
        string HostUri { get; set; }
        string PathUri { get; set; }
        string Summary { get; set; }
        DateTime Date { get; set; }
        string GUID { get; set; }
    }
}
